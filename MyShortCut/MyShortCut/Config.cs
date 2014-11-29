using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MyShortCut
{
    public class Config
    {
        public ArrayList table = new ArrayList();

        public int width = 133;
        public int height = 300;
 

        public ApplicationGroup getGroup(String groupText)
        {
            for (int i = 0, j = table.Count; i < j; i++)
            {
                ApplicationGroup group = (ApplicationGroup)table[i];
                if (group.text.Equals(groupText))
                {
                    return group;
                }
            }
            return null;
        }

        public void clear()
        {
            table.Clear();
            table = new ArrayList();
        }
    }
}
