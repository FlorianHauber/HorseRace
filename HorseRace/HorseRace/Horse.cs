using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
// Florian Hauber
// 1IHIF
// 24.6.2026
// description: horse class
//
namespace HorseRace
{
    public class Horse
    {
        private int _number;
        private string _name;
        private int _age;
        private int _position = 1;
        private int _rank;

        public Horse(int number, string name, int age)
        {
            _number = number;
            _name = name;
            _age = age;
        }

        public void SetNumber(int number)
        {
            _number = number;
        }
        public int GetNumber()
        {
            return _number;
        }

        public void SetName(string name)
        {
            _name = name;
        }
        public string GetName()
        {
            return _name;
        }

        public void SetAge(int age)
        {
            _age = age;
        }
        public int GetAge()
        {
            return _age;
        }

        public void SetPositon(int position)
        {
            _position = position;
        }
        public int GetPosition()
        {
            return _position;
        }

        public void SetRank(int rank)
        {
            _rank = rank;
        }
        public int GetRank()
        {
            return _rank;
        }

        public void Move()
        {
            _position++;
        }
    }
}
