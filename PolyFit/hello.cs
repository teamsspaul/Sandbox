using System;
class HelloWorld {
  static void ShowVector(int[] vector)
  {
    Console.Write("   ");
    for (int i = 0; i < vector.Length; ++i)
      Console.Write(vector[i] + " ");
    Console.WriteLine("\n");
  }
  static double[][] MatrixInverse(double[][] matrix)
  {
    // assumes determinant is not 0
    // that is, the matrix does have an inverse
    int n = matrix.Length;
    double[][] result = MatrixCreate(n, n); // make a copy of matrix
    for (int i = 0; i < n; ++i)
      for (int j = 0; j < n; ++j)
        result[i][j] = matrix[i][j];

    double[][] lum; // combined lower & upper
    int[] perm;
    int toggle;
    toggle = MatrixDecompose(matrix, out lum, out perm);

    double[] b = new double[n];
    for (int i = 0; i < n; ++i)
    {
      for (int j = 0; j < n; ++j)
        if (i == perm[j])
          b[j] = 1.0;
        else
          b[j] = 0.0;
 
      double[] x = Helper(lum, b); // 
      for (int j = 0; j < n; ++j)
        result[j][i] = x[j];
    }
    return result;
  } // MatrixInverse
  static int MatrixDecompose(double[][] m, out double[][] lum, out int[] perm)
  {
    // Crout's LU decomposition for matrix determinant and inverse
    // stores combined lower & upper in lum[][]
    // stores row permuations into perm[]
    // returns +1 or -1 according to even or odd number of row permutations
    // lower gets dummy 1.0s on diagonal (0.0s above)
    // upper gets lum values on diagonal (0.0s below)

    int toggle = +1; // even (+1) or odd (-1) row permutatuions
    int n = m.Length;
    // make a copy of m[][] into result lu[][]
    lum = MatrixCreate(n, n);
    for (int i = 0; i < n; ++i)
      for (int j = 0; j < n; ++j)
        lum[i][j] = m[i][j];
    // make perm[]
    perm = new int[n];
    for (int i = 0; i < n; ++i)
      perm[i] = i;
    for (int j = 0; j < n - 1; ++j) // process by column. note n-1 
    {
      double max = Math.Abs(lum[j][j]);
      int piv = j;
      for (int i = j + 1; i < n; ++i) // find pivot index
      {
        double xij = Math.Abs(lum[i][j]);
        if (xij > max)
        {
          max = xij;
          piv = i;
        }
      } // i
      if (piv != j)
      {
        double[] tmp = lum[piv]; // swap rows j, piv
        lum[piv] = lum[j];
        lum[j] = tmp;
        int t = perm[piv]; // swap perm elements
        perm[piv] = perm[j];
        perm[j] = t;
        toggle = -toggle;
      }
      double xjj = lum[j][j];
      if (xjj != 0.0)
      {
        for (int i = j + 1; i < n; ++i)
        {
          double xij = lum[i][j] / xjj;
          lum[i][j] = xij;
          for (int k = j + 1; k < n; ++k)
            lum[i][k] -= xij * lum[j][k];
        }
      }
    } // j
    return toggle;
  } // MatrixDecompose
  static double[] Helper(double[][] luMatrix, double[] b) // helper
  {
    int n = luMatrix.Length;
    double[] x = new double[n];
    b.CopyTo(x, 0);

    for (int i = 1; i < n; ++i)
    {
      double sum = x[i];
      for (int j = 0; j < i; ++j)
      sum -= luMatrix[i][j] * x[j];
      x[i] = sum;
    }
    x[n - 1] /= luMatrix[n - 1][n - 1];
    for (int i = n - 2; i >= 0; --i)
    {
      double sum = x[i];
      for (int j = i + 1; j < n; ++j)
        sum -= luMatrix[i][j] * x[j];
      x[i] = sum / luMatrix[i][i];
    }
    return x;
  }
  static double [][] MatrixCreate(int rows,int cols)
  {
    double [][] result = new double[rows][];
    for (int i = 0; i < rows; ++i)
      result[i] = new double[cols];
    return result;
  }
  static double [][] MatrixProduct(double [][] matrixA, double [][] matrixB)
  {
    int aRows = matrixA.Length;
    int aCols = matrixA[0].Length;
    int bRows = matrixB.Length;
    int bCols = matrixB[0].Length;
    if (aCols != bRows)
      throw new Exception("Non-conformable matrices");
    double[][] result = MatrixCreate(aRows, bCols);
    for (int i = 0; i < aRows; ++i)
      for (int j = 0; j < bCols; ++j)
        for (int k = 0; k < aCols; ++k)
          result[i][j] += matrixA[i][k] * matrixB[k][j];
    return result;
  }
  static string MatrixAsString(double[][] matrix)
  {
    string s = "";
    for (int i = 0; i < matrix.Length; ++i)
    {
      for (int j = 0; j < matrix[i].Length; ++j)
        s += matrix[i][j].ToString("F3").PadLeft(8) + " ";
      s += Environment.NewLine;
    }
    return s;
  }
  static string MatrixAsStringE(double[][] matrix)
  {
    string s = "";
    for (int i = 0; i < matrix.Length; ++i)
    {
      for (int j = 0; j < matrix[i].Length; ++j)
        s += matrix[i][j].ToString("E3").PadLeft(8) + " ";
      s += Environment.NewLine;
    }
    return s;
  }
  static string ListAsString(double[] List)
  {
    string s = "";
    for (int i = 0;i < List.Length;++i)
      s += List[i].ToString("F3") + " ";
    s += Environment.NewLine;
    return s;
  }
  static string ListAsStringE(double[] List)
  {
    string s = "";
    for (int i = 0;i < List.Length;++i)
      s += List[i].ToString("E3") + " ";
    s += Environment.NewLine;
    return s;
  }
  static double MatrixDeterminant(double[][] matrix)
  {
    double[][] lum;
    int[] perm;
    int toggle = MatrixDecompose(matrix, out lum,out perm);
    double result = toggle;
    for (int i = 0; i < lum.Length; ++i)
      result *= lum[i][i];
    return result;
  }
  static double[][] ExtractLower(double[][] lum)
  {
    int n = lum.Length;
    double[][] result = MatrixCreate(n, n);
    for (int i = 0; i < n; ++i)
    {
      for (int j = 0; j < n; ++j)
      {
        if (i == j)
          result[i][j] = 1.0;
        else if (i > j)
          result[i][j] = lum[i][j];
      }
    }
    return result;
  }
  static double[][] ExtractUpper(double[][] lum)
  {
    int n = lum.Length;
    double[][] result = MatrixCreate(n, n);
    for (int i = 0; i < n; ++i)
    {
      for (int j = 0; j < n; ++j)
      {
        if (i <= j)
          result[i][j] = lum[i][j];
      }
    }
    return result;
  }
  static double[][] Transpose(double[][] Mat)
  {
    int Rows = Mat.Length;
    int Cols = Mat[0].Length;
    double[][] result = MatrixCreate(Cols, Rows);
    for (int row = 0;row < Rows;++row)
      for (int col = 0;col<Cols;++col)
        result[col][row] = Mat[row][col];
    return result;
  }
  static void Main() {
    //Console.WriteLine("Collect GateWidths");
    string text = System.IO.File.ReadAllText(@"Input80.txt");
    string[] Lines = text.Split("\n");
    double[] GateWidths = new double[Lines.Length-1];
    double[] GateWidthsSorted = new double[Lines.Length-1];
    double[][] Uncertainty = MatrixCreate(Lines.Length-1,1);
    double[][] UncertaintySorted = MatrixCreate(Lines.Length-1,1);
    for(int i=0;i<Lines.Length-1;++i)
    {
      GateWidths[i] = Convert.ToDouble(Lines[i].Split(",")[0]);
      GateWidthsSorted[i] = Convert.ToDouble(Lines[i].Split(",")[0]);
      Uncertainty[i][0] = Convert.ToDouble(Lines[i].Split(",")[1]);
      
    }
    //Console.WriteLine("Sort GateWidths");
    Array.Sort(GateWidthsSorted);
    for (int i=0;i<Lines.Length-1;++i)
    {
      for (int j=0;j<Lines.Length-1;++j)
      {
        if(GateWidths[j]==GateWidthsSorted[i])
	{
	  UncertaintySorted[i][0] = Uncertainty[j][0];
	}
      }
    }

    //Console.WriteLine("GateWidths = ");
    //Console.WriteLine(ListAsStringE(GateWidthsSorted));
    Console.WriteLine("Uncertainty = ");
    Console.WriteLine(MatrixAsStringE(UncertaintySorted));

    //Create the X Matrix
    int Order = 9;
    int Cols  = Order+1;
    int Rows  = GateWidths.Length;
    double[][] X = MatrixCreate(Rows,Cols);
    for (int row = 0;row<Rows;++row)
      for (int col = 0;col<Cols;++col)
        X[row][col] = Math.Pow(GateWidthsSorted[row],col);
    //Console.WriteLine("X Matrix is");
    //Console.WriteLine(MatrixAsStringE(X));

    //Create XT Matrix
    double[][] XT = Transpose(X);
    //Console.WriteLine("XT Matrix is");
    //Console.WriteLine(MatrixAsStringE(XT));

    //Solve for coefficients
    double[][] a = MatrixProduct(MatrixProduct(MatrixInverse(MatrixProduct(XT,X)),XT),Uncertainty);
    Console.WriteLine("Coefficients");
    Console.WriteLine(MatrixAsStringE(a));
    //double[][] m = MatrixCreate(4, 4);
    //m[0][0] = 3; m[0][1] = 7; m[0][2] = 2; m[0][3] = 5;
    //m[1][0] = 1; m[1][1] = 8; m[1][2] = 4; m[1][3] = 2;
    //m[2][0] = 2; m[2][1] = 1; m[2][2] = 9; m[2][3] = 3;
    //m[3][0] = 5; m[3][1] = 4; m[3][2] = 7; m[3][3] = 1;
    //Console.WriteLine("Original matrix m is ");
    //Console.WriteLine(MatrixAsString(m));
    //double[][] inv = MatrixInverse(m);
    //Console.WriteLine("Inverse matrix inv is ");
    //Console.WriteLine(MatrixAsString(inv));
  }
}