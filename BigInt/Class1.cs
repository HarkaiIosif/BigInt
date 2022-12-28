using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BigInt
{
    internal class Bint
    {
        private string Read;
        private List<byte> digits;
        private int sign = 0;
        private bool ridn = false;
        public Bint()
        {
            digits = new List<byte>();
            sign = 1;
            Read = string.Empty;
        }
        public Bint(string v)
        {
            this.Read = v;
            if (!valid(v)) throw new Exception("Wrong imput");
            sign = 1;
            if (Read[0] == '-') sign = -1;
            if (Read[0] == '+' || Read[0] == '-') Read = Read.Substring(1);
            digits = new List<byte>();
            for (int i = Read.Length - 1; i >= 0; i--)
            {
                digits.Add((byte)(Read[i] - '0'));
            }

        }
        private bool valid(string v)
        {
            string correct = "+-1234567890";
            foreach (var item in v) if (correct.IndexOf(item) == -1) return false;
            return true;
        }
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (sign == -1) output.Append("-");
            if(ridn==true) output.Append("1/");
            for (int i = digits.Count - 1; i >= 0; i--)
                output.Append(digits[i].ToString());
            return output.ToString();
        }
        public static bool operator ==(Bint a, Bint b)
        {

            if (a.sign != b.sign) return false;
            if(a.digits.Count !=b.digits.Count) return false;
            for (int i = a.digits.Count - 1, j = b.digits.Count - 1; i>=0||j>=0 ; i--, j--)
            {
                if(a.digits[i] != b.digits[j]) return false;
            }
            return true;
            
        }
        public static bool  operator !=(Bint a,Bint b)
        {
            if (a.sign != b.sign) return true;
            if (a.digits.Count != b.digits.Count) return true;
            for (int i = a.digits.Count - 1, j = b.digits.Count - 1; (i > 0) && (j > 0); i--, j--)
            {
                if (a.digits[i] != b.digits[j]) return true;
            }
            return false;
        }
        public static bool operator >(Bint a, Bint b)
        {
            if(a.sign==-1&& b.sign==1) return false;
            if (a.sign == 1 && b.sign == -1) return true;
            if (a == b) return false;
            if (a.sign == b.sign && a.digits.Count != b.digits.Count) if (a.digits.Count > b.digits.Count) return true;
            for(int i=a.digits.Count-1;i>=0;i--)
            {
                if (a.digits[i]<b.digits[i]) return false;
                if (a.digits[i] > b.digits[i]) return true;
            }
            return true;
        }
        public static bool operator <(Bint a, Bint b)
        {
            if (a == b) return false;
            if (a > b == false) return true;
            else return false;
        }
        public static bool operator <=(Bint a,Bint b)
        {
            if (a < b || a == b) return true;
            else return false;
        }
        public static bool operator >=(Bint a,Bint b)
        {
            if (a > b || a == b) return true;
            else return false;
        }
        public static Bint operator +(Bint a,Bint b)
        {
            int o1, o2,carry=0;
            Bint result=new Bint();
            if (a.sign == -1 && b.sign == -1) result.sign = -1;
            else result.sign = 1;
            for(int i = 0, j = 0; i < a.digits.Count || j < b.digits.Count || carry == 1; i++, j++)
            {
                o1 = i < a.digits.Count ? a.digits[i] : 0;
                o2=j<b.digits.Count ? b.digits[j] : 0;
                result.digits.Add((byte)((o1+o2+carry)%10));
                carry = (o1 + o2 + carry) / 10;
            }
            if(a.sign == -1&&b.sign == 1)
            {
                a.sign = 1;
                result = b - a;
                if (a < b) result.sign = 1;
                else result.sign = -1;
                a.sign = -1;
            }
            if (a.sign == 1 && b.sign == -1)
            {
                b.sign = 1;
                result = a - b;
                b.sign = -1;
            }
            return result;
        }
        public static Bint operator -(Bint a, Bint b)
        {
            int o1, o2;
            byte aux=0;
            bool switched = false;
            Bint o = new Bint("0");
            Bint result = new Bint();
            if (a == o && b == o) return o;
            if (a == b) return o;
            if (a == o)
            {
                result.sign = -1;
                result.digits = b.digits;
                return result;
            }
            if (b == o) return a;
            if (a.sign == 1 && b.sign == 1 && a < b) {
                Bint copy = a;
                a = b;
                b=copy;
                result.sign = -1;
                switched = true;
                
            }
            if (a.sign == 1 && b.sign == 1) 
            for (int i = 0, j = 0; i < a.digits.Count || j < b.digits.Count ; i++, j++)
            {
                o1 = i < a.digits.Count ? a.digits[i] : 0;
                o2 = j < b.digits.Count ? b.digits[j] : 0;
                if (o1 < o2) { result.digits.Add((byte)(10 + o1 - o2 ));
                            if(o1==0)
                            {
                                if(i+1<a.digits.Count) aux = a.digits[i + 1];
                                int t = i + 1;
                                if (aux == 0) for ( t=i+1 ; t < a.digits.Count; t++)
                                    {
                                        aux=a.digits[t+1];
                                        a.digits.RemoveAt(t);
                                        a.digits.Insert(t,9);
                                        if(aux!=0) break;
                                    }
                            if (t + 1 < a.digits.Count)
                            {
                                a.digits.RemoveAt(t + 1);

                                a.digits.Insert(t + 1, (byte)(aux - 1));
                            }
                            }
                        else
                        {
                            aux = a.digits[i + 1];
                            a.digits.RemoveAt(i + 1);
                            a.digits.Insert(i + 1, (byte)(aux - 1));
                        }
                }
                else result.digits.Add((byte)(o1 - o2));
            }
            if (a.sign == -1 && b.sign == 1)
            {
                a.sign = 1;
                result = a + b;
                result.sign = -1;
                a.sign = -1;
                return result;
                
                
            }
            if (a.sign == -1 && b.sign == -1)
            {   a.sign = 1;
                b.sign = 1;
                result = b - a;
                if (a < b) result.sign = -1;
                a.sign = -1;
                b.sign = -1;
            }
            if (a.sign == 1 && b.sign == -1)
            {
                b.sign = 1;
                result = a + b;
                b.sign = -1;
            }
            for (int i = result.digits.Count - 1; i >= 0; i--) if (result.digits[i] == 0) result.digits.RemoveAt(i);
                else break;
            if (switched)
            {
                Bint copy = a;
                a = b;
                b = copy;
                switched = false;
            }
            return result;
            
                    
            
        }
        public static Bint operator *(Bint a, Bint b)
        {   Bint result = new Bint();
            Bint o=new Bint("0");
            if (a == o || b == o) return o;
            for (int i = 0; i < a.digits.Count + b.digits.Count; i++) result.digits.Add(0); 
            int o1, o2, carry = 0,aux,aux1=0;
            for(int i = 0; i < b.digits.Count; i++)
            { o1 = i < b.digits.Count ? b.digits[i] : 0;
                for (int j = 0,k=i; j < a.digits.Count&&k<a.digits.Count+b.digits.Count||carry!=0; j++,k++)
                {
                    o2 = j < a.digits.Count ? a.digits[j] : 0;
                    aux = result.digits[k];
                    result.digits.RemoveAt(k);
                    result.digits.Insert(k, (byte)((o1 * o2 % 10) + aux + carry));
                    carry = o1 * o2 / 10;
                    if (result.digits[k]>9)
                    {
                        aux = result.digits[k];
                        result.digits.RemoveAt(k);
                        result.digits.Insert(k, (byte)(aux % 10));
                        aux1=result.digits[k+1];
                        result.digits.RemoveAt(k + 1);
                        result.digits.Insert(k+1,(byte)(aux1+aux/10));
                    }
                    

                }
            }
            if ((a.sign == 1 && b.sign == 1) || (a.sign == -1 && b.sign == -1)) result.sign = 1;
            else result.sign = -1;
            for (int i = result.digits.Count-1; i >=0; i--) if (result.digits[i] == 0) result.digits.RemoveAt(i);
                else break;
            return result;
        }
        public static Bint operator ^(Bint a,Bint b)
        {   Bint result=new Bint();
            Bint o = new Bint("0");
            Bint d=new Bint("1");
            if (b == o) return d;
            if (b.sign == -1) { b.sign = 1;
                result.ridn = true;
            }
            result = a;
            for (Bint i = d; i < b; i = i + d) result = result * a;
            if (a.sign == -1) if (b.digits[b.digits.Count - 1] % 2 == 0) result.sign = 1;
                else result.sign = -1;
            else result.sign = 1;
            if (result.ridn==true) b.sign = -1;
            return result;

        }

    }
}

