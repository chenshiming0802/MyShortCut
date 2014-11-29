using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MyShortCut
{
    public class ApplicationGroup
    {
        public String text;
        public ArrayList cells = new ArrayList();

        public ApplicationCell getCell(String key){
            for(int i=0,j=cells.Count;i<j;i++)
            {
                ApplicationCell cell = (ApplicationCell)cells[i];
                if (cell.text.Equals(key))
                {
                    return cell;
                }
            }
            return null;
        }
    }
}
