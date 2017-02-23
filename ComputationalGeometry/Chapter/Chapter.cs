using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public enum ChapterType
    {
        ConvexHull,
    }

    public class Chapter
    {
        public string[] algorithmNames;
        public virtual void OpenFile() { }
        public virtual void SaveFile() { }
        public virtual void Init() { }
        public virtual void Reset() { }
        public void Go(int algorithmIndex) {
            OnGo(algorithmIndex);
            DrawResult();
        }
        public virtual void OnGo(int algorithmIndex) { }
        public virtual void DrawResult() { }
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
