/***********************************************************/
//---模    块：主成分分析
//---功能描述：载入数据，设置参数
//---编码时间：2018-11-06
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace xxkUI.AnalysisMethods
{
   public class PCA
    {
        /// <summary>
        /// 求实对称阵的 特征值 和 特征向量
        /// </summary>
        /// <param name="data">实对称阵</param>
        /// <param name="num">维数</param>
        /// <param name="eigenvalue">引用参数 特征值 回传</param>
        /// <param name="eigenvector">引用参数 特征向量 回传</param>
        /// <returns>是否成功</returns>
        /// 
        //public static bool GetCovariancematrix(double[,] data, int num, ref double[] eigenvalue, ref double[,] eigenvector)

        public static bool GetCovariancematrix(List<double[]> sourceMatrix, int totalRowNum, int totalColumnNum, ref double[] eigenvalue, ref double[,] eigenvector)
        {
            #region //矩阵的标准化
            double[,] middleMatrix = new double[totalRowNum, totalColumnNum];//用中间存储临时矩阵
            for (int j = 0; j < totalColumnNum; j++)
            {
                double col1 = 0.0;//初始化为0
                for (int i = 0; i < totalRowNum; i++)
                {
                    col1 += sourceMatrix[i][j];
                }
                double mean = col1 / totalRowNum;//这一列的平均值
                //下面算列的每一个数-平均值，存在一个数组里面
                for (int m = 0; m < totalRowNum; m++)
                {
                    middleMatrix[m, j] = sourceMatrix[m][j] - mean;
                }

            }//到这一步，可以得到一个 差 的矩阵。


            //下一步，计算middleMatrix里的每一列的平方和 除以 （行数-1）
            for (int i = 0; i < totalColumnNum; i++)
            {
                double col = 0.0;

                for (int j = 0; j < totalRowNum; j++)
                {
                    col = col + middleMatrix[j, i] * middleMatrix[j, i];//得到了平方和

                }

                double squar = col / (totalRowNum - 1);//得到了方差

                double sqrt1 = Math.Sqrt(squar);//标准差
                // Console.WriteLine(sqrt1);
                // varMatrix[i] = sqrt1;//存储了标准差，一维数组。
                for (int m = 0; m < totalRowNum; m++)
                {
                    middleMatrix[m, i] = middleMatrix[m, i] / sqrt1;//存储了标准化的最后结果
                }
            }

            #endregion //标准化结束 下一步求协方差矩阵

            #region//求协方差矩阵
            double[,] timeMatrix = new double[totalColumnNum, totalColumnNum];
            for (int j = 0; j < totalColumnNum; j++)
            {
                double col1 = 0.0;//初始化为0
                for (int i = 0; i < totalRowNum; i++)
                {
                    col1 += middleMatrix[i, j];
                }
                double mean = col1 / totalRowNum;//这一列的平均值
                //下面算列的每一个数-平均值，存在一个数组里面
                for (int m = 0; m < totalRowNum; m++)
                {
                    middleMatrix[m, j] = middleMatrix[m, j] - mean;
                }

            } //各列减去自己的平均值 下一步求相关系数矩阵

            for (int i = 0; i < totalColumnNum; i++)
            {
                for (int j = 0; j < totalColumnNum; j++)
                {
                    double sum = 0.0;
                    for (int m = 0; m < totalRowNum; m++)
                    {
                        sum = sum + middleMatrix[m, i] * middleMatrix[m, j];//得到的是，一列和二列的相应位置相乘之后的和，
                    }
                    timeMatrix[i, j] = sum / (totalRowNum - 1);
                }
            }

            #endregion

            #region 矩阵特征值和特征向量的计算
            try
            {
                double[,] A = timeMatrix;

                //E 单位标准矩阵   存储 特征向量--------------------------------------------
                double[,] V = new double[totalColumnNum, totalColumnNum];
                for (int iv = 0; iv < totalColumnNum; iv++)
                {
                    for (int iv2 = 0; iv2 < totalColumnNum; iv2++)
                    {
                        if (iv == iv2)
                        {
                            V[iv, iv2] = 2;
                        }
                        else
                        {
                            V[iv, iv2] = 2;
                        }
                    }
                }
                //----------------------------------------------


                double[] eigsv = new double[totalColumnNum];//存储 特征值
                for (int ieigsv = 0; ieigsv < totalColumnNum; ieigsv++)
                {
                    eigsv[ieigsv] = 0;
                }


                double epsl = 0.0001;
                int maxt = 10;
                int n = totalColumnNum;


                double tao, t, cn, sn; // 临时变量
                double maxa;// 记录非对角线元素最大值

                //------------------------------------------------------------------------------------------------
                for (int it = 0; it < maxt; it++)
                {
                    maxa = 0;
                    for (int p = 0; p < n - 1; p++)
                    {
                        for (int q = p + 1; q < n; q++)
                        {
                            if (Math.Abs(A[p, q]) > maxa) // 记录非对角线元素最大值
                            {
                                maxa = Math.Abs(A[p, q]);
                            }
                            if (Math.Abs(A[p, q]) > epsl) // 非对角线元素非0时才执行Jacobi变换
                            {
                                // 计算Givens旋转矩阵的重要元素:cos(theta), 即cn, sin(theta), 即sn
                                tao = 0.5 * (A[q, q] - A[p, p]) / A[p, q];

                                if (tao >= 0) // t为方程 t^2 + 2*t*tao - 1 = 0的根, 取绝对值较小的根为t
                                {
                                    t = -tao + Math.Sqrt(1 + tao * tao);
                                }
                                else
                                {
                                    t = -tao - Math.Sqrt(1 + tao * tao);
                                }
                                cn = 1 / Math.Sqrt(1 + t * t);
                                sn = t * cn;

                                // Givens旋转矩阵之转置左乘A, 即更新A的p行和q行
                                for (int j = 0; j < n; j++)
                                {
                                    double apj = A[p, j];
                                    double aqj = A[q, j];
                                    A[p, j] = cn * apj - sn * aqj;
                                    A[q, j] = sn * apj + cn * aqj;
                                }

                                // Givens旋转矩阵右乘A, 即更新A的p列和q列
                                for (int i = 0; i < n; i++)
                                {
                                    double aip = A[i, p];
                                    double aiq = A[i, q];
                                    A[i, p] = cn * aip - sn * aiq;
                                    A[i, q] = sn * aip + cn * aiq;
                                }

                                // 更新特征向量存储矩阵V, V=J0×J1×J2...×Jit, 所以每次只更新V的p, q两列
                                for (int i2 = 0; i2 < n; i2++)
                                {
                                    double vip = V[i2, p];
                                    double viq = V[i2, q];
                                    V[i2, p] = cn * vip - sn * viq;
                                    V[i2, q] = sn * vip + cn * viq;
                                }
                            }

                        }

                    }




                    if (maxa < epsl) // 非对角线元素已小于收敛标准，迭代结束
                    {

                        break;
                    }
                }
                //-----------------------------------------------------------------------------------------------------

                // 特征值向量  排序
                for (int j2 = 0; j2 < n; j2++)
                {
                    eigsv[j2] = A[j2, j2];
                    //fprintf(fp2, "%f ",eigsv[j2]);
                }
                // 对特征值向量从大到小进行排序, 并调整特征向量顺序 (直接插入法)
                double[] tmp = new double[n];
                for (int j = 1; j < n; j++)
                {
                    int i = j;
                    double a = eigsv[j];
                    for (int k = 0; k < n; k++)
                    {
                        tmp[k] = V[k, j];
                    }
                    while (i > 0 && eigsv[i - 1] < a)
                    {
                        eigsv[i] = eigsv[i - 1];
                        for (int k = 0; k < n; k++)
                        {
                            V[k, i] = V[k, i - 1];
                        }
                        i--;
                    }
                    eigsv[i] = a;
                    for (int k2 = 0; k2 < n; k2++)
                    {
                        V[k2, i] = tmp[k2];
                    }
                }
                //----------------------------------------------------------------------------------------------------------
                //打印特征值 与 特征向量    数据回传
                for (int ivc = 0; ivc < totalColumnNum; ivc++)
                {
                    for (int ivc2 = 0; ivc2 < totalColumnNum; ivc2++)
                    {

                        //fprintf(fp2, "%f%s", V[ivc, ivc2], "\n");
                        eigenvector[ivc, ivc2] = V[ivc, ivc2];

                    }

                }


                for (int ieigsvc = 0; ieigsvc < totalColumnNum; ieigsvc++)
                {

                    //fprintf(fp4, "%f%s", eigsv[ieigsvc], "\n");
                    eigenvalue[ieigsvc] = eigsv[ieigsvc];

                }
                //----------------------------------------------------------------------------------------------------------
                return true;
            }
            catch
            {
                return false;
            }
            #endregion

        } 

    }
}
