﻿using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class TagsTests
    {
        [Test]
        public void Test_can_tag_a_project()
        {
            var result = Tags.Create(new TagCreate
            {
                Name = "v0.5",
                Message = "Test message",
                Ref = "master",
                ReleaseDescription = "Test description",
            });

            Assert.IsNotNull(result);
            Assert.IsNotNull(Tags.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", System.StringComparison.Ordinal)));
            Assert.IsNotNull(Tags.All.FirstOrDefault(x => string.Equals(x.Message, "Test message", System.StringComparison.Ordinal)));

            Tags.Delete("v0.5");
            Assert.IsNull(Tags.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", System.StringComparison.Ordinal)));
        }

        [Test]
        public void Test_can_create_a_release()
        {
            var result = Tags.Create(new TagCreate
            {
                Name = "0.7",
                Ref = "master",
            });

            var release = Tags.CreateRelease("0.7", new ReleaseCreate() { Description = "test" });
            Assert.That(release.TagName, Is.EqualTo("0.7"));
            Assert.That(release.Description, Is.EqualTo("test"));

            release = Tags.UpdateRelease("0.7", new ReleaseUpdate() { Description = "test edited" });
            Assert.That(release.TagName, Is.EqualTo("0.7"));
            Assert.That(release.Description, Is.EqualTo("test edited"));

            Tags.Delete("0.7");
            Assert.IsNull(Tags.All.FirstOrDefault(x => string.Equals(x.Name, "0.7", System.StringComparison.Ordinal)));
        }

        private static ITagClient Tags
        {
            get
            {
                Assert.IsNotNull(Initialize.UnitTestProject);
                return Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Tags;
            }
        }
    }
}