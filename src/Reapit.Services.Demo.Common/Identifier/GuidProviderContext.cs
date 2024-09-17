﻿using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Reapit.Services.Demo.Common.Identifier;

/// <summary>Execution context for the <see cref="Identifier.GuidProvider"/> class</summary>
/// <remarks>This is excluded from code coverage as it's behaviour is tested through <see cref="Identifier.GuidProvider" /></remarks>
[ExcludeFromCodeCoverage]
public class GuidProviderContext : IDisposable
{
    internal Guid NewGuid;
    private static readonly ThreadLocal<Stack> ThreadScopeStack = new (() => new Stack());

    /// <summary>
    /// Initializes a new instance of the <see cref="GuidProviderContext"/> class
    /// </summary>
    /// <param name="newGuid"></param>
    public GuidProviderContext(Guid newGuid)
    {
        NewGuid = newGuid;
        ThreadScopeStack.Value?.Push(this);
    }

    /// <summary>
    /// The timestamp configured for the current execution context
    /// </summary>
    public static GuidProviderContext? Current
    {
        get
        {
            if ((ThreadScopeStack.Value?.Count ?? 0) == 0)
                return null;
            
            return ThreadScopeStack.Value?.Peek() as GuidProviderContext;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ThreadScopeStack.Value?.Pop();
        GC.SuppressFinalize(this);
    }
}