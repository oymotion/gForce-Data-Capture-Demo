///////////////////////////////////////////////////////////
//  GameControllerManager.cs
//  Implementation of the Class GameControllerManager
//  Generated by Enterprise Architect
//  Created on:      02-2鏈?2021 16:07:33
//  Original author: hebin
///////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace GameCtrler
{
    /// <summary>
    /// GameController管理类
    /// </summary>
    public class GameControllerManager
    {
        private static Dictionary<int, IGameController> gameControllers = new Dictionary<int, IGameController>();


        public GameControllerManager()
        {

        }

        ~GameControllerManager()
        {

        }

        /// 
        /// <param name="className"></param>
        public static IGameController CreateGameController(string className)
        {
            IGameController ret = null;

            if (className.Equals("GForceGameController"))
            {
                ret = (IGameController)new GForceGameController();
            }

            return ret;
        }

        /// 
        /// <param name="id">手柄编号</param>
        /// <param name="gameController">GameController对象</param>
        public static void SetGameController(int id, IGameController gameController)
        {
            gameControllers.Add(id, gameController);
        }

        /// 
        /// <param name="id">手柄编号，0开始顺序编号</param>
        public static IGameController GetGameController(int id)
        {
            if (gameControllers.Count == 0)
            {
                return null;
            }

            return gameControllers[id];
        }

        public static Dictionary<int, IGameController> GetGameControllers()
        {
            return gameControllers;
        }

    }//end GameControllerManager

}//end namespace GameCtrler