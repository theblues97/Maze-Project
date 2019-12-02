using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ResearchFinal
{
    public struct Note
    {
        public Point point;
         public int loc;
        public Note(Point Point, int Loc)
        {
            this.point = Point;
            this.loc = Loc;
        }
    }

    public struct Stack
    {
        public int top;
        public List<Note> notes;
        public Stack(int Top, List<Note> Notes)
        {
            this.top = Top;
            this.notes = Notes;
        }
    }

    public class StackController
    {
        private Stack stack = new Stack();
        internal Stack Stack { get => stack; set => stack = value; }

        public void Init()
        {
            stack.top = 0;
            stack.notes = new List<Note>();
        }

        public bool isEmpty()
        {
            return (stack.top == 0);
        }

        public void Push(Note point)
        {
            stack.notes.Add(point);
            stack.top++;
        }

        public Note Peak()
        {
            return stack.notes[stack.top - 1];
        }

        public Note Pop()
        {
            Note note = new Note();
            if (!isEmpty())
            {
                note = stack.notes[stack.top - 1];
                stack.notes.RemoveAt(stack.top - 1);
                stack.top--;              
            }
            return note;
        }
    }

}
