using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskManager
{
    class GLB
    {
        public int employeeID { get; set; }
        public bool isChanged { get; set; }
        public bool isConnection { get; set; }
        public bool isRight { get; set; }

        public GLB()
        {
            employeeID = 0;
            isChanged = false;
            isConnection = false;
            isRight = false;
        }
    }
}
