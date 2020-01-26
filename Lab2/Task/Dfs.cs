using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DepthFirstSearch.Task
{
    public class Dfs
    {
        public CheckerField StartField { set; get; }
        public CheckerField FinalField { set; get; }
        public List<string> PathHistory { set; get; }
        public List<Move> Path { set; get; }

        public string GetPath
        {
            get { return string.Join(",\n", PathHistory); }
        }

        public void Run()
        {
            StartField = new CheckerField();
            StartField.InitStartFieldCells();

            FinalField = new CheckerField();
            FinalField.InitEndFieldCells();

            Path = new List<Move>();
            PathHistory = new List<string>();
        }

        private int counter = 0;
        public void Step()
        {
            if (StartField.Equals(FinalField)) return;
            Cell freeCell = StartField.Cells.First(x => x.Content == null);
            List<Move> availableMoves = StartField.Moves.Where(x => x.HasCell(freeCell)).ToList();
            foreach (var move in availableMoves)
            {
                if(move.Checker.Moves.Exists(x => x.Equals(move))) continue;
                Path.Add(move);
                move.Transfer();

                if (StartField.Equals(FinalField)) return;
                #region Rollback
                if (Path.Count > 15)
                {
                    Path.RemoveAt(Path.Count - 1);
                    move.RollBack();
                    return;
                }
                #endregion
                Step();
                if (StartField.Equals(FinalField)) return;
                Path.RemoveAt(Path.Count - 1);
                move.RollBack();
            }
        }

        public void InitPath()
        {
            StartField.InitPath();
            Path = StartField.Path;
            foreach (var move in Path)
            {
                move.Transfer();
            }
        }
    }
}
