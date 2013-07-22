using System;
using System.Text;
using System.Windows.Forms;

//using BigInteger = System.Numerics.BigInteger;

namespace dotTraceUtility
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            txtName.Text = Environment.MachineName;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            BigInteger n = Parse("5852679291121824840551871814155310876070738979171116374633");
            BigInteger p = Parse("76968069673302466210259040287");
            BigInteger q = Parse("76040354343872999195552087159");

            BigInteger z = (p - 1) * (q - 1);

            BigInteger u = new BigInteger(Encoding.Default.GetBytes(txtName.Text));
            u |= 1;

            BigInteger d = u.modInverse(z);

            int iUserHash = UserHash(txtName.Text, String.Empty);

            // 时间
            BigInteger m = new BigInteger((long)65535);
            m <<= 0x38;
            m += new BigInteger((long)iUserHash); // 用户名和公司的校验
            // CustomerId
            m += new BigInteger((Int32)1) << 0x58;
            //// Type
            //m += new BigInteger((Int16)1) << 0x20;
            // Version
            m += new BigInteger((Int32)0xFA0) << 0x10;
            // ProductVersion
            m += new BigInteger((Int16)0xFA0) << 0x48;
            //// Edition
            //m += new BigInteger((Int32)1) << 0x78;

            BigInteger lic = m.modPow(d, n);
            txtKey.Text = Convert.ToBase64String(lic.getBytes());
        }

        BigInteger Parse(String str)
        {
            return new BigInteger(str, 10);
        }

        // UserHash ，直接从反编译的源代码中抄过来的 
        // 作用是判断用户名与License是否匹配 
        private int UserHash(String username, String company)
        {
            int i = 0;
            for (int j = 0; j < username.Length; j++)
            {
                i = ((i << 7) + username[j]) % 65521;
            }

            for (int k = 0; k < company.Length; k++)
            {
                i = ((i << 7) + company[k]) % 65521;
            }
            return (i);
        }
    }
}
