﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;

using System.Reflection;

using SiliconStudio.Core.Reflection;
using SiliconStudio.Quantum.References;

namespace SiliconStudio.Quantum.Contents
{
    /// <summary>
    /// An implementation of <see cref="IContent"/> that gives access to a member of an object.
    /// </summary>
    public class MemberContent : ContentBase
    {
        protected IContent Container;

        public MemberContent(IContent container, IMemberDescriptor member, ITypeDescriptor descriptor, bool isPrimitive, IReference reference)
            : base(member.Type, descriptor, isPrimitive, reference)
        {
            if (container == null) throw new ArgumentNullException("container");
            Member = member;
            Container = container;
        }

        /// <summary>
        /// The <see cref="IMemberDescriptor"/> used to access the member of the container represented by this content.
        /// </summary>
        public IMemberDescriptor Member { get; protected set; }

        /// <inheritdoc/>
        public sealed override object Value
        {
            get
            {
                if (Container.Value == null) throw new InvalidOperationException("Container's value is null");
                return Member.Get(Container.Value);
            }
            set
            {
                if (Container.Value == null) throw new InvalidOperationException("Container's value is null");
                var containerValue = Container.Value;
                Member.Set(containerValue, value);

                if (Container.Value.GetType().GetTypeInfo().IsValueType)
                    Container.Value = containerValue;
            }
        }
    }
}
