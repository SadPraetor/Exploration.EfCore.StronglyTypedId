using ArrayExtensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace StronglyTypedId.Models
{
	public static class ContractExtensions
	{
		/// <summary>
		/// Clones contract
		/// </summary>
		/// <param name="contract"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static (Contract Clone, Dictionary<object, object> EntityMap) Clone(this Contract contract)
		{
			if (contract is null)
			{
				throw new ArgumentNullException(nameof(contract));
			}
			var output = contract.Clone<Contract>()!;
			var clone = output.Clone;

			clone.Id = default;
			clone.Branch = ContractBranch.Revision;
			contract.Branch = ContractBranch.UnderRevision;
			return output;
		}

		private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance)!;
		public static (TEntity? Clone, Dictionary<object, object> EntityMap) Clone<TEntity>(this TEntity originalObject) where TEntity : class
		{
			var entityMap = new Dictionary<object, object>(new ReferenceEqualityComparer());
			var clone = (TEntity?)InternalCopy(originalObject, entityMap);
			return (clone, entityMap);
		}

		public static bool IsPrimitive(this Type type)
		{
			if (type == typeof(string)) return true;
			return type.IsValueType & type.IsPrimitive;
		}

		private static object? InternalCopy(object originalObject, IDictionary<object, object> visited)
		{
			if (originalObject == null)
				return null;
			var typeToReflect = originalObject.GetType();
			if (typeToReflect.IsPrimitive())
				return originalObject;
			if (visited.ContainsKey(originalObject))
				return visited[originalObject];
			if (typeof(Delegate).IsAssignableFrom(typeToReflect))
				return null;
			if (typeof(ILazyLoader).IsAssignableFrom(typeToReflect))
				return null;


			var cloneObject = CloneMethod.Invoke(originalObject, null);

			if (typeToReflect.IsArray)
			{
				var arrayType = typeToReflect.GetElementType();
				if (!arrayType.IsPrimitive())
				{
					Array clonedArray = (Array)cloneObject;
					clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
				}

			}
			visited.Add(originalObject, cloneObject);
			CopyFields(originalObject, visited, cloneObject, typeToReflect);
			RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
			return cloneObject;
		}

		private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
		{
			if (typeToReflect.BaseType != null)
			{
				RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
				CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
			}
		}

		private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
		{
			foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
			{
				if (filter != null && filter(fieldInfo) == false) continue;
				if (fieldInfo.FieldType.IsPrimitive()) continue;
				var originalFieldValue = fieldInfo.GetValue(originalObject);
				var clonedFieldValue = InternalCopy(originalFieldValue, visited);
				fieldInfo.SetValue(cloneObject, clonedFieldValue);
			}
		}
	}

	public class ReferenceEqualityComparer : EqualityComparer<object>
	{
		public override bool Equals(object x, object y)
		{
			return ReferenceEquals(x, y);
		}
		public override int GetHashCode(object obj)
		{
			if (obj == null) return 0;
			return obj.GetHashCode();
		}
	}
}

namespace ArrayExtensions
{
	public static class ArrayExtensions
	{
		public static void ForEach(this Array array, Action<Array, int[]> action)
		{
			if (array.LongLength == 0) return;
			ArrayTraverse walker = new ArrayTraverse(array);
			do action(array, walker.Position);
			while (walker.Step());
		}
	}

	internal class ArrayTraverse
	{
		public int[] Position;
		private int[] maxLengths;

		public ArrayTraverse(Array array)
		{
			maxLengths = new int[array.Rank];
			for (int i = 0; i < array.Rank; ++i)
			{
				maxLengths[i] = array.GetLength(i) - 1;
			}
			Position = new int[array.Rank];
		}

		public bool Step()
		{
			for (int i = 0; i < Position.Length; ++i)
			{
				if (Position[i] < maxLengths[i])
				{
					Position[i]++;
					for (int j = 0; j < i; j++)
					{
						Position[j] = 0;
					}
					return true;
				}
			}
			return false;
		}
	}
}


