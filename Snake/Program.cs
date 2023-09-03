using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
     
    static class fuction
    {
        static public Random random = new Random();
    }

    class pos
    {
        public pos(pos inp)
        {
            x = inp.x;
            y = inp.y;
            fill = inp.fill;
        }
        public pos(int max_x,int max_y,string fi="",bool rad=false)
        {

            if (rad)
            {
                x = fuction.random.Next(1, max_x-1);
                y = fuction.random.Next(1, max_y-1);
            }
            else
            {
                x = max_x; y = max_y;
            }
            fill = fi;
        }


        public int x { get; private set; }
        public int y { get; private set; }

        private string fill;

        public void setPos(pos p,bool esr=true)
        {
            if(esr) print(" ");
            this.x = p.x;
            this.y = p.y;
            print(fill);
        }

        public bool coll(pos inp)
        {
            return (inp.x == this.x) && (inp.y == this.y);
        }
        public bool rang(pos inp)
        {
            return (((inp.x < this.x) && (inp.y < this.y)) && (0 <= inp.x)) && (0 <= inp.y);
        }

        public bool coll(int i_x,int i_y)
        {
            return (i_x == this.x) && (i_y == this.y);
        }

        public void print(string s)
        {
            Console.SetCursorPosition(x + 1, y + 1);
            Console.Write(s);
        }

        public static pos operator +(pos p1,pos p2)
        {
            pos re = new pos(p1.x + p2.x, p1.y + p2.y);
            return re;
        }
        public static pos operator -(pos p1, pos p2)
        {
            pos re = new pos(p1.x - p2.x, p2.y - p2.y);
            return re;
        }
        public static pos operator !(pos p1)
        {
            pos re = new pos(-p1.x ,-p1.y);
            return re;
        }
    }

    class Game
    {
        public Game(int x, int y)
        {
            size = new pos(x, y,"#");
            body = new List<pos>();
            direction = new pos(0, 1);

            body.Add(new pos(x / 2, y / 2,"@"));
            food = new pos(x, y,"+", true);
            soc = 0;

        }
        static pos size;

        int soc;

        pos direction;

        pos food;
        List<pos> body;

        public void food_rad()
        {
            int[,] arr = new int[size.x,size.y]; //all zero
            List<pos> rad_ls = new List<pos>();

            foreach (var item in body)
                arr[item.x, item.y] = 1;

            for (int xp = 0; xp < size.x; xp++)
                for (int yp = 0; yp < size.y; yp++)
                    if (arr[xp, yp] == 0) rad_ls.Add(new pos(xp, yp));

            if (rad_ls.Count > 0)
                food.setPos(rad_ls[fuction.random.Next(0, rad_ls.Count - 1)], false);

        }

        public bool run(ConsoleKey inp)
        {
            pos buff_inp = null;
            switch (inp)
            {
                case ConsoleKey.UpArrow:
                    if (inp == ConsoleKey.DownArrow) break;
                    buff_inp = new pos(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    if (inp == ConsoleKey.UpArrow) break;
                    buff_inp = new pos(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    if (inp == ConsoleKey.RightArrow) break;
                    buff_inp = new pos(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    if (inp == ConsoleKey.LeftArrow) break;
                    buff_inp = new pos(1, 0);
                    break;
                default:
                    break;
            }

            if (buff_inp != null)
                if (direction.coll(!buff_inp) == false) direction = buff_inp;

            pos next = body[0] + direction;
            if (!size.rang(next)) return false;
            foreach (var item in body)
                if (item.coll(next)) return false;

            for (int i = body.Count; i > 0; i--)
            {
                if (i == 1) 
                    if(body.Count==1) body[0].setPos(next);
                    else body[0].setPos(next, false);
                else if(i == body.Count) body[i - 1].setPos(body[i - 2]);
                else body[i - 1].setPos(body[i - 2], false);
            }

            if (food.coll(next))
            {
                if (body.Count == 1) body.Add(new pos(body[body.Count - 1].x, body[body.Count - 1].y,"O"));
                else body.Add(new pos(body[body.Count - 1]));
                food_rad();
                soc++;
            }

            food.print("+");

            return true;
        }

        public override string ToString()
        {
            string s = "";
            for (int y = -1; y < size.y + 2; y++)
            {
                s += "#";
                for (int x = 0; x < size.x + 1; x++)
                {
                    if ((y == -1) || (y == size.y + 1)) { s += "#"; }
                    else
                    {
                        int type = 0;
                        foreach (var item in body)
                            if (item.coll(x, y)) type = 1;
                        if (food.coll(x, y)) type = 2;
                        if (body[0].coll(x, y)) type = 3;

                        switch (type)
                        {
                            case 0:
                                s += " ";
                                break;
                            case 1:
                                s += "O";
                                break;
                            case 2:
                                s += "+";
                                break;
                            case 3:
                                s += "@";
                                break;
                        }

                    }
                }
                s += "#\n";
            }

            //s += "x:" + body[0].x + "\n";
            //s += "y:" + body[0].y;
            s += "score:" + soc;
            return s;
        }
        
        public void drawBox()
        {

            Console.SetCursorPosition(0, 0);

            string s = "";
            for (int y = -1; y < size.y+1; y++)
            {
                s += "#";
                for (int x = 0; x < size.x + 1; x++)
                    if ((y == -1) || (y == size.y)) s += "#";
                    else s += " ";

                s += "#\n";
            }

            Console.WriteLine(s);
            body[0].print("@");
            food.print("+");

        }

    }



    class Program
    {


        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            int delay_time = 300;
            bool notdead = true;
            ConsoleKey inp = ConsoleKey.RightArrow;
            Game g = new Game(80, 25);
            g.drawBox();

            Task read_key = new Task(() => {

                while (notdead)
                {
                    if (Console.KeyAvailable)
                        inp = Console.ReadKey().Key;

                }


            });

            read_key.Start();

            while (notdead)
            {
                System.Threading.Thread.Sleep(50);
                notdead = g.run(inp);
            } 

            

            /*
            Console.WriteLine("123456");
            Console.WriteLine("123456");
            Console.ReadKey();
            Console.SetCursorPosition(0,0);
            Console.WriteLine("###");

            Console.ReadKey();
            */
        }
    }
}
