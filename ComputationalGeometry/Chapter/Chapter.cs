using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public enum ChapterType
    {
        None,
        ConvexHull,
    }

    public class Chapter
    {
        public virtual void OpenFile() { }
        public virtual void SaveFile() { }
        public virtual void Init() { }
        public virtual void Reset() { }
    }

    public static class SimpleChapterFactory
    {
        public static Chapter CreateChapter(ChapterType type) {
            switch (type){
                case ChapterType.ConvexHull:
                    return new ConvexHullChapter();
            }
            return null;
        }
    }
}
