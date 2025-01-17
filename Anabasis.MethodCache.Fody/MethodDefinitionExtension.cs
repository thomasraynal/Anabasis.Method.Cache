﻿using System;
using System.Linq;
using Mono.Cecil;

namespace Anabasis.MethodCache.Fody
{
	//https://github.com/SpatialFocus/MethodCache.Fody/blob/master/src/SpatialFocus.MethodCache.Fody/Extensions/MethodDefinitionExtension.cs
	public static class MethodDefinitionExtension
	{
		public static bool HasCacheAttribute(this MethodDefinition methodDefinition, References references)
		{
			var cacheAttributeType = references.CacheAttributeType.Resolve();

			return methodDefinition.CustomAttributes.Any(
				classAttribute => classAttribute.AttributeType.Resolve().Equals(cacheAttributeType));
		}

		public static CustomAttribute GetCacheAttribute(this MethodDefinition methodDefinition, References references)
		{
			var cacheAttributeType = references.CacheAttributeType.Resolve();

			return methodDefinition.CustomAttributes.FirstOrDefault(
				classAttribute => classAttribute.AttributeType.Resolve().Equals(cacheAttributeType));
		}

		public static bool IsEligibleForWeaving(this MethodDefinition methodDefinition, References references)
		{

			var typeDefinition = references.CompilerGeneratedAttributeType.Resolve();

			if (methodDefinition.ReturnType.Equals(methodDefinition.Module.TypeSystem.Void) || 
				methodDefinition.ReturnType.Equals(references.TaskTypeReference) ||
				methodDefinition.ReturnType.FullName.Equals(references.ValueTaskTypeReference.FullName))
			{
				return false;
			}

			var hasOutParameter = methodDefinition.Parameters.Any(parameterDefinition => parameterDefinition.IsOut);

			var isSpecialName = methodDefinition.IsSpecialName || methodDefinition.IsGetter || methodDefinition.IsSetter ||
				methodDefinition.IsConstructor;

			var hasCompilerGeneratedAttribute =
				methodDefinition.CustomAttributes.Any(attribute => attribute.AttributeType.Resolve().Equals(typeDefinition));

			return !hasOutParameter && !isSpecialName && !hasCompilerGeneratedAttribute;
		}
	}
}
