﻿////////////////////////////////////////////////////////////////////////////////////////////////
//
//          #######
//          #   ##    ####   #####    #####  ##  ##   #####
//             ##    ##  ##  ##  ##  ##      ##  ##  ##
//            ##  #  ######  ##  ##   ####   ##  ##   ####
//           ##  ##  ##      ##  ##      ##   #####      ##
//          #######   ####   ##  ##  #####       ##  #####
//                                           #####
//          Z-Wave, the wireless language.
//
//          Copyright Zensys A/S, 2006
//
//          All Rights Reserved
//
//          Description:        Code generation base class
//
//          Author:             Lars Damsgaard, Glaze A/S
//
//          Last Changed By:    $Author: Lars Damsgaard, Glaze A/S $
//          Revision:           $Revision: 1.0 $
//          Last Changed:       $Date: 2006/11/27 $
//
//////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Utils;
using ZWave.Xml.Application;

namespace ZWave.Xml.HeaderGenerator
{
    /// <summary>
    /// Base class for C# class file and C header file generation
    /// </summary>
    public abstract class Generator
    {
        /// <summary>
        /// Reference to options class
        /// </summary>
        protected string Version { get; set; }

        /// <summary>
        /// Reference to options class
        /// </summary>
        protected Options Options1 { get; set; }

        /// <summary>
        /// Reference to basic device list
        /// </summary>
        protected IEnumerable<BasicDevice> BasicDeviceList { get; set; }

        /// <summary>
        /// Reference to generic device list
        /// </summary>        
        protected IList<GenericDevice> GenericDeviceList { get; set; }

        /// <summary>
        /// Reference to common specific device list
        /// </summary>
        protected IList<SpecificDevice> CommonSpecificDeviceList { get; set; }

        /// <summary>
        /// Reference to the command class list
        /// </summary>
        protected IList<CommandClass> CommandClassList { get; set; }

        /// <summary>
        /// Array of control fields in the code template
        /// </summary>
        protected ArrayList ControlFieldList { get; set; }

        /// <summary>
        /// Stream reader instance for the code template
        /// </summary>
        protected TextReader Sr { get; set; }

        /// <summary>
        /// Stream writer instance for the generated C# code
        /// </summary>
        protected TextWriter Sw { get; set; }

        /// <summary>
        /// Generates Code using StreamWriter (sw) by reading from template file (using StreamReader sr) and by reading 
        /// from the data structures representing the XML (basicDeviceList, genericDeviceList, commandClassList).
        /// </summary>
        public abstract void Generate(string optionsChTemplateFile, string optionsDefaultFileName, bool keepLegacyExceptions, bool isSystem);

        /// <summary>
        /// If a template line starts with this string, then the entire line
        /// is not used for code generation.
        /// </summary>
        protected const string LineCommentIdentifier = "%";

        /// <summary>
        /// Determines if a line is a comment (starting with LineCommentIdentifier).
        /// </summary>
        /// <param name="line"></param>
        /// <returns>True if line is a comment</returns>
        protected bool LineIsComment(string line)
        {
            int p = line.IndexOf(LineCommentIdentifier, StringComparison.Ordinal);
            if (p > 0)
            {
                string s = line.Substring(0, p + 1).TrimStart(null);
                if (s.Substring(0, LineCommentIdentifier.Length) == LineCommentIdentifier)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the specified line contains a source template control field
        /// </summary>
        /// <param name="controlFieldName">Name of control field - if found</param>
        /// <param name="line">Source line that may contain a control field</param>
        /// <returns>True, if the control field name is found in the specified line</returns>
        protected bool LineHasControlField(ref string controlFieldName, string line)
        {
            foreach (string s in ControlFieldList)
            {
                string controlField = "<!" + s + "!>";
                if (line.Contains(controlField))
                {
                    controlFieldName = s;
                    return true;
                }
            }
            return false;
        }





        /// <summary>
        /// Performs "Count" string concatenations.
        /// Example: 
        /// Str="ABC" Count=2  
        /// Result in "ABCABC"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected string DuplicateStr(string str, int count)
        {
            string result = "";
            for (int i = 0; i < count; i++)
            {
                result += str;
            }
            return result;
        }

        /// <summary>
        /// Creates 4 space characters for each tabLevel
        /// </summary>
        /// <param name="tabLevel">Number of space fields for each tab</param>
        /// <returns></returns>
        protected string MakeTabs(int tabLevel)
        {
            return DuplicateStr(" ", 4 * tabLevel);
        }

        /// <summary>
        /// Gets the name of the type struct.
        /// </summary>
        /// <param name="cmdClass">The CMD class.</param>
        /// <param name="cmd">The CMD.</param>
        /// <param name="byteStr">The byte STR.</param>
        /// <param name="versionPostfix">The version postfix.</param>
        /// <returns></returns>
        protected string GetTypeStructNameCmd(CommandClass cmdClass, Command cmd, string byteStr, string versionPostfix)
        {
            return Tools.CutString(cmd.Name + byteStr + versionPostfix, Options1.ChCommandClassPrefix);
        }

        protected string GetTypeStructNameVg(CommandClass cmdClass, Param vgParam, string byteStr, string versionPostfix)
        {
            return Tools.CutString(vgParam.ParentCmd.Name + byteStr + versionPostfix, Options1.ChCommandClassPrefix);
        }

        /// <summary>
        /// Gets the type struct caption.
        /// </summary>
        /// <param name="cmdClass">The CMD class.</param>
        /// <param name="cmd">The CMD.</param>
        /// <param name="byteStr">The byte STR.</param>
        /// <param name="versionPostfix">The version postfix.</param>
        /// <returns></returns>
        protected string GetTypeStructCaptionCmd(CommandClass cmdClass, Command cmd, string byteStr, string versionPostfix)
        {
            return string.Format("/* {0} {1} */", Tools.CutUpperUnderscoreToMixedUpperLower(cmd.Name + byteStr + versionPostfix, Options1.ChCommandClassPrefix, " "), "command class structs");
        }
        protected string GetTypeStructCaptionVg(CommandClass cmdClass, Param vgParam, string byteStr, string versionPostfix)
        {
            return string.Format("/* {0} {1} */", Tools.CutUpperUnderscoreToMixedUpperLower(vgParam.ParentCmd.Name + versionPostfix, Options1.ChCommandClassPrefix, " "), "variant group structs");
        }
    }
}