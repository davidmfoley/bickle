//using System.Collections.Generic;
//using System.Linq;
//using JetBrains.Metadata.Reader.API;
//using JetBrains.ProjectModel;
//using JetBrains.ReSharper.UnitTestFramework;
//using NUnit.Framework;
//using Rhino.Mocks;

//namespace Bickle.ReSharper.Tests
//{
//    [TestFixture]
//    public class exploring_assembly
//    {
//        private BickleAssemblyExplorer Explorer = new BickleAssemblyExplorer(new BickleTestProvider());
//        private List<UnitTestElement> _addedElements;

//        [SetUp]
//        public void SetUpContext()
//        {
//            _addedElements = new List<UnitTestElement>();
        
//            var assembly = MockRepository.GenerateMock<IMetadataAssembly>();
//            var t = MockRepository.GenerateMock<IMetadataTypeInfo>();
//            t.Stub(x => x.FullyQualifiedName).Return(typeof (ExampleSpecType).FullName);

//            assembly.Stub(x => x.GetTypes()).Return(new[] {t});

//            var project = MockRepository.GenerateStub<IProject>();
            
//            Explorer.ExploreAssembly(assembly, project, Consumer);
//        }

//        [Test]
//        public void has_an_it()
//        {
//            Assert.That(_addedElements.Any(e=>e.ShortName == "Bar"));
//        }
//        private void Consumer(UnitTestElement unittestelement)
//        {
//            _addedElements.Add(unittestelement);
//        }
//    }

//    public class ExampleSpecType : Spec
//    {
//        public ExampleSpecType()
//        {
//            Describe("Foo", () =>
//            {
//                It("Bar", ()=>{});
//            });
//        }
//    }

//    [TestFixture]
//    public class NAME
//    {
//        [Test]
//        public void foo()
//        {
//            Assert.IsTrue(false);
//        }
//    }
//}
