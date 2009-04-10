using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DotNetMarche.PhotoAlbum.Model.PhotoRoutines;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.PhotoAlbum.Test.Model.PhotoRoutine
{
   [TestFixture]
   public class ResizeTest
   {
      [Test]
      public void ResizeSmaller()
      {
         PhotoSizeManager sut = new PhotoSizeManager(100, 100);
         Size size = new Size(50, 50);
         Assert.That(sut.Resize(size), Is.EqualTo(size));
      }

      [Test]
      public void ResizeSame()
      {
         PhotoSizeManager sut = new PhotoSizeManager(100, 100);
         Size size = new Size(100, 100);
         Assert.That(sut.Resize(size), Is.EqualTo(size));
      }

      [Test]
      public void ResizeProportional()
      {
         PhotoSizeManager sut = new PhotoSizeManager(100, 100);
         Size size = new Size(200, 200);
         Assert.That(sut.Resize(size), Is.EqualTo(new Size(100, 100)));
      }

      [Test]
      public void ResizeNotProportionalExceedingWidth()
      {
         PhotoSizeManager sut = new PhotoSizeManager(100, 100);
         Size size = new Size(200, 150);
         Assert.That(sut.Resize(size), Is.EqualTo(new Size(100, 75)));
      }

      [Test]
      public void ResizeNotProportionalExceedingHeight()
      {
         PhotoSizeManager sut = new PhotoSizeManager(100, 100);
         Size size = new Size(150, 200);
         Assert.That(sut.Resize(size), Is.EqualTo(new Size(75, 100)));
      }

      [Test]
      public void ResizeRectangle()
      {
         PhotoSizeManager sut = new PhotoSizeManager(800, 600);
         Size size = new Size(1000, 600);
         Assert.That(sut.Resize(size), Is.EqualTo(new Size(800, 480)));
      }
   }
}
