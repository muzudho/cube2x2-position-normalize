namespace Grayscale.Cube2X2PositionNormalize
{
    /// <summary>
    /// 正規化します。
    /// </summary>
    public class Normalizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Normalizer"/> class.
        /// </summary>
        public Normalizer()
        {
            this.IsomorphicPosition = new Position[64];
            this.IsomorphicX = new int[64];
            this.IsomorphicY = new int[64];
            this.IsomorphicZ = new int[64];
        }

        /// <summary>
        /// Gets 同型の局面は 64個。
        /// </summary>
        public Position[] IsomorphicPosition { get; private set; }

        /// <summary>
        /// Gets X方向に 90°回した回数。
        /// </summary>
        public int[] IsomorphicX { get; private set; }

        /// <summary>
        /// Gets Y方向に 90°回した回数。
        /// </summary>
        public int[] IsomorphicY { get; private set; }

        /// <summary>
        /// Gets Z方向に 90°回した回数。
        /// </summary>
        public int[] IsomorphicZ { get; private set; }

        /// <summary>
        /// 正規化をする。
        /// つまり、64個の局面を作り、そのうち代表的な１つを選ぶ。
        /// </summary>
        /// <param name="sourcePosition">元となる局面。</param>
        /// <param name="handle">回す箇所。</param>
        /// <returns>局面と、回す箇所。</returns>
        public (Position, int) Normalize(Position sourcePosition, int handle)
        {
            for (int i = 0; i < 64; i++)
            {
                var pos = Position.Clone(sourcePosition);
                this.IsomorphicPosition[i] = pos;

                if (i % 4 == 1)
                {
                    // +X
                    pos.RotateView(2);
                    this.IsomorphicX[i] += 1;
                }
                else if (i % 4 == 2)
                {
                    // +X
                    pos.RotateView(2);
                    pos.RotateView(2);
                    this.IsomorphicX[i] += 2;
                }
                else if (i % 4 == 3)
                {
                    // +X
                    pos.RotateView(2);
                    pos.RotateView(2);
                    pos.RotateView(2);
                    this.IsomorphicX[i] += 3;
                }

                if ((i / 4) % 4 == 1)
                {
                    // +Y
                    pos.RotateView(0);
                    this.IsomorphicY[i] += 1;
                }
                else if ((i / 4) % 4 == 2)
                {
                    // +Y
                    pos.RotateView(0);
                    pos.RotateView(0);
                    this.IsomorphicY[i] += 2;
                }
                else if ((i / 4) % 4 == 3)
                {
                    // +Y
                    pos.RotateView(0);
                    pos.RotateView(0);
                    pos.RotateView(0);
                    this.IsomorphicY[i] += 3;
                }

                if ((i / 16) % 4 == 1)
                {
                    // +Z
                    pos.RotateView(1);
                    this.IsomorphicZ[i] += 1;
                }
                else if ((i / 4) % 4 == 2)
                {
                    // +Z
                    pos.RotateView(1);
                    pos.RotateView(1);
                    this.IsomorphicZ[i] += 2;
                }
                else if ((i / 4) % 4 == 3)
                {
                    // +Z
                    pos.RotateView(1);
                    pos.RotateView(1);
                    pos.RotateView(1);
                    this.IsomorphicZ[i] += 3;
                }
            }

            var isomorphicText = new string[64];
            for (int i = 0; i < 64; i++)
            {
                isomorphicText[i] = this.IsomorphicPosition[i].BoardText;
            }

            // 辞書順ソートする。
            System.Array.Sort(isomorphicText);

            /*
            // 確認表示。
            for (int i = 0; i < 64; i++)
            {
                Trace.WriteLine(string.Format(
                    CultureInfo.CurrentCulture,
                    "{0} {1}",
                    i,
                    isomorphicText[i]));
            }
            */

            // 代表の局面。
            var normalizedBoardText = isomorphicText[0];

            // 何番目のものか。
            int normalizedIndex = 0;
            for (; normalizedIndex < 64; normalizedIndex++)
            {
                if (normalizedBoardText == isomorphicText[normalizedIndex])
                {
                    break;
                }
            }

            // 今回 回そうと思っていたハンドルを変更します。
            var normalizedHandle = this.RotateView(normalizedIndex, handle);

            return (Position.Parse(normalizedBoardText), normalizedHandle);
        }

        /// <summary>
        /// 今回 回そうと思っていたハンドルを変更します。
        /// </summary>
        /// <param name="isomorphicIndex">同型の要素番号。</param>
        /// <param name="handle">回す箇所。</param>
        /// <returns>視角を変えたあとの、回す箇所。</returns>
        public int RotateView(int isomorphicIndex, int handle)
        {
            var rotateButtonGroup = new RotateButtonGroup();
            for (int iView = 0; iView < this.IsomorphicX[isomorphicIndex]; iView++)
            {
                // +X
                rotateButtonGroup.RotateView(2);
            }

            for (int iView = 0; iView < this.IsomorphicY[isomorphicIndex]; iView++)
            {
                // +Y
                rotateButtonGroup.RotateView(0);
            }

            for (int iView = 0; iView < this.IsomorphicZ[isomorphicIndex]; iView++)
            {
                // +Z
                rotateButtonGroup.RotateView(1);
            }

            // 視角を変えたので、対応するハンドルの位置が変わる。
            return rotateButtonGroup.LabelArray[handle];
        }
    }
}
