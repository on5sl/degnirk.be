using System;
using System.Collections.Generic;

namespace Service
{
    public interface IGoogleService
    {
        List<List<KeyValuePair<string, string>>> GetEvents(DateTime from, DateTime to);
    }
}