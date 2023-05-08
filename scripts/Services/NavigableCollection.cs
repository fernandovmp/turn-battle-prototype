using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace TurnBattle.Services
{
    public class NavigableCollection<T>
    {
        private readonly IEnumerable<T> _list;
        private int _currentIndex;

        public NavigableCollection(IEnumerable<T> list)
        {
            _list = list;
            _currentIndex = 0;
        }

        public T Current => _list.ElementAt(_currentIndex);

        public void Next()
        {
            _currentIndex = (_currentIndex + 1) % _list.Count();
        }

        public void Previous()
        {
            int count = _list.Count();
            if(count <= 1)
            {
                _currentIndex = 0;
                return;
            }
            _currentIndex -= 1;
            if(_currentIndex < 0)
            {
                _currentIndex = count + _currentIndex;
                
            }
        }
    }
}