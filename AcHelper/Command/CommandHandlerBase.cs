﻿using BuerTech.Utilities.Logger;
using System;

namespace AcHelper.Command
{
    /// <summary>
    /// The CommandHandlerBase provides base functionalities for the class containing AutoCAD commands. 
    /// If you use this feature, make sure you have created an instance of the <see cref="AcHelper.Logging"/>.LogWriter first.
    /// </summary>
    public class CommandHandlerBase
    {
        private const string WHITESPACE = " ";
        private const string NEWLINE = "\n";

        /// <summary>
        /// Executes the given <see cref="IAcadCommand"/>IAcadCommand class as a command.
        /// When an unhandled exception occurs, it will be caught and thrown as a dialog.
        /// This way the the chance for an AutoCAD crash stays minimal.
        /// </summary>
        /// <typeparam name="T">Class based on interface IAcadCommand</typeparam>
        public static void ExecuteCommand<T>() where T : IAcadCommand
        {
            try
            {
                var cmd = Activator.CreateInstance<T>();
                cmd.Execute();
            }
            catch (System.Exception ex)
            {
                ExceptionHandler.ShowDialog(ex, true, true);

                Logger.WriteToLog(ex, LogPrior.Critical);
                ////if (AcLog.IsInitialized)
                ////{
                ////    AcLog.Logger.WriteToLog(ex, BuerTech.Utilities.Logger.LogPrior.Critical); 
                ////}
            }
        }
        /// <summary>
        /// Executes a command from the commandline.
        /// </summary>
        /// <param name="cmd">Command name.</param>
        /// <param name="parameters">Optional parameters.</param>
        public static void ExecuteFromCommandLine(string cmd, params object[] parameters)
        {
            ExecuteFromCommandLine(true, cmd, parameters);
        }
        /// <summary>
        /// Executes a command from the commandline.
        /// </summary>
        /// <param name="echo">true to echo command in commandline.</param>
        /// <param name="cmd">Command name.</param>
        /// <param name="parameters">Optional parameters.</param>
        public static void ExecuteFromCommandLine(bool echo, string cmd, params object[] parameters)
        {
            // Prepare Command and potential parameters.
            string execute = cmd;
            execute += NEWLINE;
            if (parameters != null && parameters.Length > 0)
            {
                // if parameters are present
                // add every parameter to the command.
                foreach (var item in parameters)
                {
                    // end parameter with a whitespace so it will be passed through.
                    execute += item.ToString() + WHITESPACE;
                }
            }

            try
            {
                // Execute
                Active.Document.SendStringToExecute(execute, true, false, echo);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ShowDialog(ex, true, true);

                Logger.WriteToLog(ex, LogPrior.Error);
                ////if (AcLog.IsInitialized)
                ////{
                ////    AcLog.Logger.WriteToLog(ex, BuerTech.Utilities.Logger.LogPrior.Critical);
                ////}
            }
        }
    }
}
