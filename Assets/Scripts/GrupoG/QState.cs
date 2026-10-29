using NavigationDJIA.World;
using System;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;

/// <summary>
/// TODO(alumno):
/// Define el "estado" que usará la Tabla Q para identificar cada situación del agente.
/// 
/// El estado debe contener toda la información necesaria para que el agente pueda
/// tomar decisiones informadas. Tú decides qué características incluir según lo
/// que consideres relevante para resolver el problema.
/// 
/// Ejemplos típicos de información que puede formar un estado:
///   - Posición del agente en la grid.
///   - Posición del otro personaje (enemigo).
///   - Distancia relativa entre agente y enemigo.
///   - Si hay muros en direcciones cercanas.
///   - Cualquier otro dato que consideres útil.
/// 
/// En este ejercicio te damos un ejemplo simple basado únicamente en las posiciones
/// del agente y del oponente. Puedes usarlo tal cual o ampliarlo.
/// 
/// IMPORTANTE: 
///  El estado debe poder convertirse a una clave única (string) mediante ToKey(),
///  ya que esa clave se usará como índice en la TablaQ y en el archivo CSV.
/// </summary>

namespace GrupoG
{
    public sealed class QState
    {
        // 0 = up, 1 = right, 2 = down, 3 = left
        public bool[] spaceAround = new bool[4];

        public Distance[] distances = new Distance[4];

        public QState(CellInfo agent, CellInfo other, WorldInfo worldInfo)
        {

            int distX = agent.x - other.x;
            int distY = agent.y - other.y;

            int farDistance = 1;

            if (distX < 0)
            {
                if (distX <= -1 * farDistance) distances[1] = Distance.Far;
                else distances[1] = Distance.Close;
                distances[3] = Distance.None;
            }
            else if (distX > 0)
            {
                if (distX >= farDistance) distances[3] = Distance.Far;
                else distances[3] = Distance.Close;
                distances[1] = Distance.None;
            }

            if(distY < 0)
            {
                if(distY <= -1 * farDistance) distances[0] = Distance.Far;
                else distances[0] = Distance.Close;
                distances[2] = Distance.None;
            }
            else if(distY > 0)
            {
                if (distX >= farDistance) distances[2] = Distance.Far;
                else distances[2] = Distance.Close;
                distances[0] = Distance.None;
            }


            spaceAround[0] = worldInfo[agent.x, agent.y + 1].Walkable;
            spaceAround[1] = worldInfo[agent.x + 1, agent.y].Walkable;
            spaceAround[2] = worldInfo[agent.x, agent.y - 1].Walkable;
            spaceAround[3] = worldInfo[agent.x - 1, agent.y].Walkable;

        }

        public string ToKey()
        {
            return $"{spaceAround[0]}, {spaceAround[1]}, {spaceAround[2]}, {spaceAround[3]} | {distances[0]}, {distances[1]}, {distances[2]}, {distances[3]}";
        }

        public enum Distance
        {
            Close,
            Far,
            None
        }
    }
}