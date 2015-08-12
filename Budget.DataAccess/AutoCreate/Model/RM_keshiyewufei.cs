using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budget.DataAccess.AutoCreate.Model
{
    [Serializable()]
    public class RM_keshiyewufei
    {

        private int kSID;
        private string depname = String.Empty;
        private decimal bQJE;
        private decimal sQJE;
        private decimal zJJE;
        private decimal kYJE;
        private decimal remark;
        private string remark1 = String.Empty;

        private string lSJL = string.Empty;

        public string LSJL
        {
            get { return lSJL; }
            set { lSJL = value; }
        }


        public RM_keshiyewufei() { }


        public int KSID
        {
            get { return this.kSID; }
            set { this.kSID = value; }
        }





        public string Depname
        {
            get { return this.depname; }
            set { this.depname = value; }
        }


        public decimal BQJE
        {
            get { return this.bQJE; }
            set { this.bQJE = value; }
        }


        public decimal SQJE
        {
            get { return this.sQJE; }
            set { this.sQJE = value; }
        }


        public decimal ZJJE
        {
            get { return this.zJJE; }
            set { this.zJJE = value; }
        }


        public decimal KYJE
        {
            get { return this.kYJE; }
            set { this.kYJE = value; }
        }


        public decimal Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }


        public string Remark1
        {
            get { return this.remark1; }
            set { this.remark1 = value; }
        }

    }
}

