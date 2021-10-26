using System;
using System.Collections.Generic;
using CosmosStack.Collections;
using CosmosStack.Validation.Projects;

namespace CosmosStack.Reflection.ObjectVisitors.Correctness
{
    internal class CorrectnessProjectManager : AbstractProjectManager
    {
        private readonly Dictionary<(int, int), IProject> _namedTypeProjects = new();
        private readonly Dictionary<int, IProject> _typedProjects = new();
        private readonly object _lockObj = new();

        public override void Register(IProject project)
        {
            if (project is null) throw new ArgumentNullException(nameof(project));

            switch (project.Class)
            {
                case ProjectClass.Typed:
                    _lockObj.LockAndRun(() =>
                        _typedProjects.AddValueOrOverride(project.Type.GetHashCode(), project)
                    );
                    break;

                case ProjectClass.Named:
                    _lockObj.LockAndRun(() =>
                        _namedTypeProjects.AddValueOrOverride((project.Type.GetHashCode(), project.Name.GetHashCode()), project)
                    );
                    break;

                default:
                    throw new InvalidOperationException("Unknown validation project.");
            }
        }

        public override IProject Resolve(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (_typedProjects.TryGetValue(type.GetHashCode(), out var result))
                return result;

            return default;
        }

        public override IProject Resolve(Type type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Resolve(type);

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (_namedTypeProjects.TryGetValue((type.GetHashCode(), name.GetHashCode()), out var result))
                return result;

            return default;
        }

        public override bool TryResolve(Type type, out IProject project)
        {
            project = Resolve(type);
            return project is not null;
        }

        public override bool TryResolve(Type type, string name, out IProject project)
        {
            project = Resolve(type, name);
            return project is not null;
        }
    }
}