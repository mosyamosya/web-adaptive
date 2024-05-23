namespace adaptweb4
{
    internal class Book
    {
        public int id;
        private string name;
        protected bool isread;
        protected internal double rate;
        internal int npages;
        public Book(int id, string name, int npages, double rate, bool isread)
        {
            this.id = id;
            this.name = name;
            this.npages = npages;
            this.rate = rate;
            this.isread = isread;
           
        }
        public void WriteTextRate()
        {
            switch (rate)
            {
                case double r when r > 8 && r <= 10:
                    Console.WriteLine("Найкраща книга у світі");
                    break;
                case double r when r > 6 && r <= 8:
                    Console.WriteLine("Непогана книга");
                    break;
                case double r when r > 4 && r <= 6:
                    Console.WriteLine("не дуже цікава книга");
                    break;
                case double r when r <= 4:
                    Console.WriteLine("Погана книга");
                    break;
            }
        }

        internal int GetNpages()
        {
            return npages;
        } 
        public void WriteInfo(Book book)
        {
            string isreadMess = isread ? "Прочитана" : "Не прочитана";
            Console.WriteLine($"{name} має {npages} сторінок, рейтинг - {rate}, статус книги для вас - {isreadMess}"); 
        }
    }
}
