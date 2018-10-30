using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts.AStarSolution
{
    
    public class AStarMind : AbstractPathMind
    {
        public bool comprobarGoals(Nodo inicial, CellInfo[] goals)
        {
            bool correcto= false;
            for (int i = 0; i < goals.Length; i++)
            {
                if (inicial.data == goals[i])
                {
                    correcto = true;
                }
            }
            return correcto;

        }
        public List<Nodo> DistanciaManhattan(Nodo inicial, CellInfo[] hijos, CellInfo[] goals, List<Nodo> open)
        {
            int x;
            int y;
            int f;
            int DistanciaM = int.MaxValue;
            for (int i = 0; i < hijos.Length; i++)
            {
                if (hijos[i] != null)
                {
                    for (int j = 0; j < goals.Length; j++)
                    {

                        x = Mathf.Abs(goals[j].RowId - hijos[i].RowId);
                        y = Mathf.Abs(goals[j].ColumnId - hijos[i].ColumnId);
                        if (x + y < DistanciaM)
                        {
                            DistanciaM = x + y;
                        }
                        f = (inicial.f + 1) + DistanciaM;
                        open.Add(new Nodo(hijos[j], DistanciaM, f, inicial));
                    }

                }
            }
            return open;
        }
        public List<Nodo> OrdenarLista(List<Nodo> open)
        {
            Nodo temp = new Nodo();             //Variable temporal.
            for (int a = 1; a < open.Count; a++)
            {
                for (int b = 0; b < open.Count - a; b++) // for(j=0; j < Nelementos-i; j++) es menor y no menor igual
                {
                    if (open[b].f > open[b + 1].f)//Condicion mayor-menor
                    {
                        temp = open[b];
                        open[b] = open[b + 1];
                        open[b + 1] = temp;
                    }
                }
            }
            return open;
        }
        public List<Nodo> expandirHijos(Nodo inicial, BoardInfo board, List<Nodo> open, CellInfo[] goals, List<Nodo> listaHijos)
        {
            CellInfo[] hijos;
            hijos = inicial.data.WalkableNeighbours(board);
            open = DistanciaManhattan(inicial, hijos, goals, open);
            open = OrdenarLista(open);
            return open;
        }
        //declarar Stack de Locomotion.MoveDirection de los movimientos hasta llegar al objetivo
        private Stack<Locomotion.MoveDirection> currentPlan = new Stack<Locomotion.MoveDirection>();
        public override void Repath()
        {
            //limpiar la Stack
            currentPlan.Clear();
            throw new NotImplementedException();
        }
        public override Locomotion.MoveDirection GetNextMove(BoardInfo board, CellInfo currentPos, CellInfo[] goals)
        {
            // si la Stack no está vacía, hacer siguiente movimiento
            if (currentPlan.Count != 0) //(currentPlan != null)
            {
                // devuelve siguiente movimiento (pop the Stack)
                return currentPlan.Pop();
            }

            // calcular camino, devuelve resultado de A*
            var searchResult = Search(board, currentPos, goals);

            // recorre searchResult y copia el camino a currentPlan
            while (searchResult.padre != null)
            {
                currentPlan.Push(searchResult.ProducedBy);
                searchResult = searchResult.padre;
            }

            // devuelve el siguiente movimiento (pop Stack)
            if (currentPlan.Any())
                return currentPlan.Pop();

            return Locomotion.MoveDirection.None;

        }

        private Nodo Search(BoardInfo board, CellInfo start, CellInfo[] goals)
        {
            var open = new List<Nodo>();
            var elementosVisitados = new List<Nodo>();
            Nodo inicial = new Nodo(start);

            open.Add(inicial);

            while(comprobarGoals(inicial, goals)==false)
            {
                open = expandirHijos(inicial, board, open, goals, elementosVisitados);
                //inicial;
            }
            

            






        }

    }
    public class Nodo{
        public CellInfo data;
        public int h;
        public int f;
        public Nodo padre;

        public Nodo(CellInfo n, int h, int f, Nodo padre)
        {
            this.data = n;
            this.h = h;
            this.f = f;
            this.padre = padre;
        }
        public Nodo(CellInfo n, Nodo padre)
        {
            this.data = n;
            this.padre = padre;
        }
        public Nodo(CellInfo n, int h)
        {
            this.data = n;
            this.h = h;
        }
        public Nodo(CellInfo n)
        {
            this.data = n;
        }
        public Nodo()
        {
        }
        public Nodo(CellInfo n, Nodo padre, int h)
        {
            this.data = n;
            this.padre = padre;
            this.h = h;
        }
    }
}