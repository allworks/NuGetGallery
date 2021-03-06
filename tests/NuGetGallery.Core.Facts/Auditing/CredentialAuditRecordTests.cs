﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Xunit;

namespace NuGetGallery.Auditing
{
    public class CredentialAuditRecordTests
    {
        [Fact]
        public void Constructor_ThrowsForNullCredential()
        {
            Assert.Throws<ArgumentNullException>(() => new CredentialAuditRecord(credential: null, removed: true));
        }

        [Fact]
        public void Constructor_ThrowsForRemovalWithNullType()
        {
            var credential = new Credential();

            Assert.Throws<ArgumentNullException>(() => new CredentialAuditRecord(credential, removed: true));
        }

        [Fact]
        public void Constructor_RemovalOfNonPasswordSetsValue()
        {
            var credential = new Credential(type: "a", value: "b");
            var record = new CredentialAuditRecord(credential, removed: true);

            Assert.Equal("b", record.Value);
        }

        [Fact]
        public void Constructor_RemovalOfPasswordDoesNotSetValue()
        {
            var credential = new Credential(type: CredentialTypes.Password.V3, value: "a");
            var record = new CredentialAuditRecord(credential, removed: true);

            Assert.Null(record.Value);
        }

        [Fact]
        public void Constructor_NonRemovalOfNonPasswordDoesNotSetsValue()
        {
            var credential = new Credential(type: "a", value: "b");
            var record = new CredentialAuditRecord(credential, removed: false);

            Assert.Null(record.Value);
        }

        [Fact]
        public void Constructor_NonRemovalOfPasswordDoesNotSetValue()
        {
            var credential = new Credential(type: CredentialTypes.Password.V3, value: "a");
            var record = new CredentialAuditRecord(credential, removed: false);

            Assert.Null(record.Value);
        }

        [Fact]
        public void Constructor_SetsProperties()
        {
            var created = DateTime.MinValue;
            var expires = DateTime.MinValue.AddDays(1);
            var lastUsed = DateTime.MinValue.AddDays(2);
            var credential = new Credential()
            {
                Created = created,
                Description = "a",
                Expires = expires,
                Identity = "b",
                Key = 1,
                LastUsed = lastUsed,
                Scopes = new List<Scope>() { new Scope(subject: "c", allowedAction: "d") },
                Type = "e",
                Value = "f"
            };
            var record = new CredentialAuditRecord(credential, removed: true);

            Assert.Equal(created, record.Created);
            Assert.Equal("a", record.Description);
            Assert.Equal(expires, record.Expires);
            Assert.Equal("b", record.Identity);
            Assert.Equal(1, record.Key);
            Assert.Equal(lastUsed, record.LastUsed);
            Assert.Equal(1, record.Scopes.Count);
            var scope = record.Scopes[0];
            Assert.Equal("c", scope.Subject);
            Assert.Equal("d", scope.AllowedAction);
            Assert.Equal("e", record.Type);
            Assert.Equal("f", record.Value);
        }
    }
}