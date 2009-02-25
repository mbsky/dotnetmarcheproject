using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotNetMarche.PhotoAlbum.Model.PhotoRoutines;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.PhotoAlbum.Test.Model.PhotoRoutine
{
   [TestFixture]
   public class FileNameGeneratorTest
   {
      [Test]
      public void TestFileNameIsCryptographic()
      {
         String generated = PhotoFileManager.GenerateName();
         Assert.That(Path.GetExtension(generated), Is.EqualTo(".jpg"));
      }

      [Test]
      public void TestFileNameUnique()
      {
         Directory.GetFiles(".\\", "*.jpg").ToList().ForEach(File.Delete);
         String[] names = new[] {"TEST", "TEST", "ANOTHER"};
         Int32 current = 0;
         using (PhotoFileManager.OverrideGenerator(() => names[current++]))
         {
            String generated1 = PhotoFileManager.GenerateName();
            File.WriteAllText(generated1, "This is a test");
            //Generate again, the generator generates the same name so the routine keeps asking for thethird time
            String generated2 = PhotoFileManager.GenerateName();
            Assert.That(generated2, Is.Not.EqualTo(generated1));
            Assert.That(current, Is.EqualTo(3));
         }

      }

      [Test]
      public void TestFileNameUniqueNotExisting()
      {
         Directory.GetFiles(".\\", "*.jpg").ToList().ForEach(File.Delete);
         String[] names = new[] { "TEST", "TEST", "ANOTHER" };
         Int32 current = 0;
         using (PhotoFileManager.OverrideGenerator(() => names[current++]))
         {
            String generated1 = PhotoFileManager.GenerateName();
            //Generate again, the generator generates the same name so the routine keeps asking for thethird time
            String generated2 = PhotoFileManager.GenerateName();
            Assert.That(generated2, Is.EqualTo(generated1));
            Assert.That(current, Is.EqualTo(2));
         }

      }

   }
}
