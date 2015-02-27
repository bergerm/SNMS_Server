using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using SNMS_Server.Variables;
using SNMS_Server.RealTimeEngine;
using SNMS_Server.RealTimeEngine.Sequences;

namespace SNMS_Server.Plugins
{
    class PluginParser
    {
        public PluginParser()
        {

        }

        bool HandleCommand( Plugin plugin,
                            Sequence sequence,
                            string sSequenceName,
                            XmlNode commandNode,
                            bool isConditionalNode,
                            ref string errorString)
        {
            string sCommandName = commandNode.Name;

            string sCommandDestination;
            XmlNode commandSource1;
            string sCommandSource1;
            bool bIsCommandSource1Variable;
            XmlNode commandSource2;
            string sCommandSource2;
            bool bIsCommandSource2Variable;

            switch (sCommandName)
            {
                case "CatenateString":
                    sCommandDestination = commandNode.SelectSingleNode("Destination").InnerText.ToLower();
                    if (plugin.GetVariable(sCommandDestination) == null)
                    {
                        errorString = "Invalid Catenate String Destination";
                        return false;
                    }

                    commandSource1 = commandNode.SelectSingleNode("Source1");
                    if (commandSource1.HasChildNodes && commandSource1.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sCommandSource1 = commandSource1.ChildNodes[0].InnerText.ToLower();
                        bIsCommandSource1Variable = true;
                        if (plugin.GetVariable(sCommandSource1) == null)
                        {
                            errorString = "Invalid Catenate String Source1";
                            return false;
                        }
                    }
                    else
                    {
                        sCommandSource1 = commandSource1.InnerText;
                        bIsCommandSource1Variable = false;
                    }

                    commandSource2 = commandNode.SelectSingleNode("Source2");
                    if (commandSource2.HasChildNodes && commandSource2.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sCommandSource2 = commandSource2.ChildNodes[0].InnerText.ToLower();
                        bIsCommandSource2Variable = true;
                        if (plugin.GetVariable(sCommandSource2) == null)
                        {
                            errorString = "Invalid Catenate String Source2";
                            return false;
                        }
                    }
                    else
                    {
                        sCommandSource2 = commandSource2.InnerText;
                        bIsCommandSource2Variable = false;
                    }

                    CatenateStringCommand catenateCommand = new CatenateStringCommand(sCommandDestination,
                                                                                sCommandSource1,
                                                                                bIsCommandSource1Variable,
                                                                                sCommandSource2,
                                                                                bIsCommandSource2Variable);
                    sequence.AddCommand(catenateCommand);
                    break;

                case "Click":
                    sCommandDestination = commandNode.SelectSingleNode("Destination").InnerText;
                    ClickElementWebDriverCommand clickCommand = new ClickElementWebDriverCommand(sCommandDestination);
                    sequence.AddCommand(clickCommand);
                    break;

                case "GetItem":
                    string sGetItemName = commandNode.Attributes["name"].Value.ToLower();
                    string sGetItemParentElement = commandNode.SelectSingleNode("ParentElement").InnerText.ToLower();
                    string sGetItemSearchType = commandNode.SelectSingleNode("SearchType").InnerText.ToLower();
                    //string sGetItemSearchValue = commandNode.SelectSingleNode("SearchValue").InnerText;
                    string sGetItemSearchValue = "";
                    bool bGetItemSearchValueVariable = false;

                    if (commandNode.HasChildNodes && commandNode.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sGetItemSearchValue = commandNode.ChildNodes[0].InnerText;
                        bGetItemSearchValueVariable = true;
                        if (plugin.GetVariable(sGetItemSearchValue) == null)
                        {
                            errorString = "Invalid Get Item Search Value";
                            return false;
                        }
                    }
                    else
                    {
                        sGetItemSearchValue = commandNode.InnerText;
                        bIsCommandSource2Variable = false;
                    }


                    if (sGetItemName == "" || sGetItemSearchType == "" || sGetItemSearchValue == "")
                    {
                        errorString = "Invalid Get Item name \"" + sGetItemName;
                        return false;
                    }

                    if (sGetItemParentElement != "" && plugin.GetWebElement(sGetItemParentElement) == null)
                    {
                        errorString = "Invalid Get Item parent element";
                        return false;
                    }

                    if (sGetItemSearchType == "id")
                    {
                        GetElementByIdWebDriverCommand getItemByID = new GetElementByIdWebDriverCommand(sGetItemParentElement,
                                                                                                            sGetItemName,
                                                                                                            sGetItemSearchValue,
                                                                                                            bGetItemSearchValueVariable);
                        sequence.AddCommand(getItemByID);
                    }
                    else if (sGetItemSearchType == "xpath")
                    {
                        GetElementByXPathWebDriverCommand getItemByXPath = new GetElementByXPathWebDriverCommand(sGetItemParentElement,
                                                                                                            sGetItemName,
                                                                                                            sGetItemSearchValue,
                                                                                                            bGetItemSearchValueVariable);
                        sequence.AddCommand(getItemByXPath);
                    }
                    break;

                case "GoBack":
                    GoBackWebDriverCommand goBackCommand = new GoBackWebDriverCommand();
                    sequence.AddCommand(goBackCommand);
                    break;

                case "GoTo":
                    string sGoToDestination;
                    bool bGoToDestIsVariable;
                    if (commandNode.HasChildNodes && commandNode.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sGoToDestination = commandNode.ChildNodes[0].InnerText.ToLower();
                        bGoToDestIsVariable = true;
                        if (plugin.GetVariable(sGoToDestination) == null)
                        {
                            errorString = "Invalid GoTo Destination";
                            return false;
                        }
                    }
                    else
                    {
                        sGoToDestination = commandNode.InnerText;
                        bGoToDestIsVariable = false;
                    }
                    GoToWebDriverCommand goToCommand = new GoToWebDriverCommand(sGoToDestination, bGoToDestIsVariable);
                    sequence.AddCommand(goToCommand);
                    break;

                case "Refresh":
                    RefreshWebDriverCommand refreshCommand = new RefreshWebDriverCommand();
                    sequence.AddCommand(refreshCommand);
                    break;

                case "Type":
                    sCommandDestination = commandNode.SelectSingleNode("Destination").InnerText.ToLower();

                    string sTypeValue;
                    bool bTypeIsVariable;
                    if (commandNode.HasChildNodes && commandNode.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sTypeValue = commandNode.SelectSingleNode("String").ChildNodes[0].InnerText.ToLower();
                        bTypeIsVariable = true;
                        if (plugin.GetVariable(sTypeValue) == null)
                        {
                            errorString = "Invalid Type Command Variable";
                            return false;
                        }
                    }
                    else
                    {
                        sTypeValue = commandNode.InnerText;
                        bTypeIsVariable = false;
                    }
                    TypeElementWebDriverCommand typeCommand = new TypeElementWebDriverCommand(sCommandDestination, sTypeValue, bTypeIsVariable);
                    sequence.AddCommand(typeCommand);
                    break;

                case "Sleep":
                    int dwSleepTime = Int32.Parse(commandNode.InnerText);
                    SleepCommand sleepCommand = new SleepCommand(dwSleepTime);
                    sequence.AddCommand(sleepCommand);
                    break;

                case "SetVariable":
                    string sSetVariableName = commandNode.SelectSingleNode("Destination").InnerText;
                    string sSetVariableValue = commandNode.SelectSingleNode("String").InnerText;
                    SetVariableCommand setVariableCommand = new SetVariableCommand(sSetVariableName, sSetVariableValue);
                    sequence.AddCommand(setVariableCommand);
                    break;

                case "CompareVariable":
                    string sCompareVariable1Name = commandNode.SelectSingleNode("Source1").InnerText;
                    string sCompareVariable2Name = commandNode.SelectSingleNode("Source2").InnerText;
                    CompareVariableCommand compareVariableCommand = new CompareVariableCommand(sCompareVariable1Name, sCompareVariable2Name);
                    sequence.AddCommand(compareVariableCommand, isConditionalNode);
                    break;

                case "IncreaseVariable":
                    string sIncreaseVariableName = commandNode.SelectSingleNode("Destination").InnerText;
                    int dwIncreaseVariableValue = Int32.Parse(commandNode.SelectSingleNode("String").InnerText);
                    IncreaseVariableCommand increaseVariableCommand = new IncreaseVariableCommand(sIncreaseVariableName, dwIncreaseVariableValue);
                    sequence.AddCommand(increaseVariableCommand);
                    break;

                case "DecreaseVariable":
                    string sDecreaseVariableName = commandNode.SelectSingleNode("Destination").InnerText;
                    int dwDecreaseVariableValue = Int32.Parse(commandNode.SelectSingleNode("String").InnerText);
                    DecreaseVariableCommand decreaseVariableCommand = new DecreaseVariableCommand(sDecreaseVariableName, dwDecreaseVariableValue);
                    sequence.AddCommand(decreaseVariableCommand);
                    break;

                default:
                    errorString = "Invalid command " + sCommandName + " in sequence " + sSequenceName;
                    return false;
            }

            return true;
        }

        public Plugin ParsePlugin(string sFileName, ref string errorString)
        {
            Plugin plugin = new Plugin();

            // Load XmlDocument
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(sFileName);

            // Get Plugin main node
            XmlNodeList pluginNodeList = xmlDocument.DocumentElement.SelectNodes("/Plugin");
            if (pluginNodeList.Count != 1)
            {
                errorString = "More than one plugin found in file";
                return null;
            }
            XmlNode pluginNode = pluginNodeList.Item(0);

            // Get Plugin Name
            XmlNodeList pluginNameNodes = pluginNode.SelectNodes("PluginName");
            if (pluginNameNodes.Count != 1)
            {
                errorString = "More than one plugin name found for plugin";
                return null;
            }
            plugin.SetPluginName(pluginNameNodes.Item(0).InnerText);

            // Get Plugin Version
            XmlNodeList pluginVersionNodes = pluginNode.SelectNodes("PluginVersion");
            if (pluginVersionNodes.Count != 1)
            {
                errorString = "More than one plugin version found for plugin";
                return null;
            }
            plugin.SetPluginVersion(pluginVersionNodes.Item(0).InnerText);

            // Get Variables
            XmlNodeList pluginVariables = pluginNode.SelectNodes("Variable");
            foreach (XmlNode variableNode in pluginVariables)
            {
                Variable variable;
                string varName = variableNode.Attributes["name"].Value.ToLower();
                string varType = variableNode.SelectSingleNode("VariableType").InnerText.ToLower();

                if (varName == "" || varType == "")
                {
                    errorString = "Invalid variable \"" + varName + "\" type \"" + varType + "\"";
                    return null;
                }

                // Create Variables
                switch (varType)
                {
                    case "string":
                        variable = new StringVariable("");
                        break;

                    case "integer":
                        variable = new IntVariable(0);
                        break;

                    default:
                        errorString = "Invalid variable \"" + varName + "\" type \"" + varType + "\"";
                        return null;
                }

                plugin.AddVariable(varName, variable);
            }

            // Get Constants
            XmlNodeList pluginConstants = pluginNode.SelectNodes("Constant");
            foreach (XmlNode variableNode in pluginConstants)
            {
                Variable variable;
                string varName = variableNode.Attributes["name"].Value.ToLower();
                string varType = variableNode.SelectSingleNode("ConstantType").InnerText.ToLower();
                string varValue = variableNode.SelectSingleNode("ConstantValue").InnerText.ToLower();

                if (varName == "" || varType == "" || varValue == "")
                {
                    errorString = "Invalid constant \"" + varName + "\" type \"" + varType + "\" value \"" + varValue + "\"";
                    return null;
                }

                // Create Constants
                switch (varType)
                {
                    case "string":
                        variable = new StringVariable(varValue, true);
                        break;

                    case "integer":
                        variable = new IntVariable(Int32.Parse(varValue), true);
                        break;

                    default:
                        errorString = "Invalid constant \"" + varName + "\" type \"" + varType + "\" value \"" + varValue + "\"";
                        return null;
                }

                plugin.AddVariable(varName, variable);
            }

            // Get Sequences
            XmlNodeList pluginSequences = pluginNode.SelectNodes("Sequence");
            foreach (XmlNode sequenceNode in pluginSequences)
            {
                string sequenceName = sequenceNode.Attributes["name"].Value;

                Sequence sequence = new Sequence(   sequenceName,
                                                    plugin.GetVariableDictionary(), 
                                                    plugin.GetWebElementsDictionary(), 
                                                    plugin.GetWebDriver()               );

                if (sequenceName == "")
                {
                    errorString = "Invalid sequence name \"" + sequenceName + "\"";
                    return null;
                }
                
                XmlNodeList commandNodes = sequenceNode.ChildNodes;

                // used to connect a label to the node index
                Dictionary<string, int> labelDictionary = new Dictionary<string, int>();
                // ued to track to which labels a specific node is connected if condition is true (or no condition). (null or nonexistent means the next item)
                Dictionary<int, string> trueConditionDetinationLabelDictionary = new Dictionary<int, string>();
                // ued to track to which labels a specific node is connected if condition is false.
                Dictionary<int, string> falseConditionDetinationLabelDictionary = new Dictionary<int, string>();

                int dwCurrentNodeIndex = 0;
                bool bIsCurrentNodeConditional = false;

                foreach (XmlNode commandNode in commandNodes)
                {
                    string sCommandName = commandNode.Name;
                    XmlNodeType nodeType = commandNode.NodeType;

                    if (nodeType != XmlNodeType.Comment)
                    {
                        switch (sCommandName.ToLower())
                        {
                            case "":
                                break;

                            case "label":
                                labelDictionary.Add(commandNode.InnerText, dwCurrentNodeIndex);
                                break;

                            case "condition":
                                trueConditionDetinationLabelDictionary.Add(dwCurrentNodeIndex, commandNode.SelectSingleNode("True").InnerText);
                                falseConditionDetinationLabelDictionary.Add(dwCurrentNodeIndex, commandNode.SelectSingleNode("False").InnerText);
                                bIsCurrentNodeConditional = true;
                                break;

                            default:
                                HandleCommand(plugin, sequence, sequenceName, commandNode, bIsCurrentNodeConditional, ref errorString);
                                dwCurrentNodeIndex++;
                                bIsCurrentNodeConditional = false;
                                break;
                        }
                    }
                }


                // Update all sequenceNodes' next values
                int dwPluginSize = dwCurrentNodeIndex;
                dwCurrentNodeIndex = 0;

                for (; dwCurrentNodeIndex < dwPluginSize; dwCurrentNodeIndex++)
                {
                    if (trueConditionDetinationLabelDictionary.ContainsKey(dwCurrentNodeIndex))
                    {
                        string sLabel = trueConditionDetinationLabelDictionary[dwCurrentNodeIndex];
                        if (!labelDictionary.ContainsKey(sLabel))
                        {
                            errorString = "Label " + sLabel + " does not exist in sequence " + sequenceName + " of plugin " + plugin.GetPluginName();
                            return null;
                        }
                        int dwNextIndex = labelDictionary[sLabel];
                        sequence.UpdateSequenceNodeNextNodeValue(dwCurrentNodeIndex, true, dwNextIndex);

                        sLabel = falseConditionDetinationLabelDictionary[dwCurrentNodeIndex];
                        if (!labelDictionary.ContainsKey(sLabel))
                        {
                            errorString = "Label " + sLabel + " does not exist in sequence " + sequenceName + " of plugin " + plugin.GetPluginName();
                            return null;
                        }
                        dwNextIndex = labelDictionary[sLabel];
                        sequence.UpdateSequenceNodeNextNodeValue(dwCurrentNodeIndex, false, dwNextIndex);
                    }

                    else
                    {
                        // Last element check
                        if (dwCurrentNodeIndex + 1 < dwPluginSize)
                        {
                            sequence.UpdateSequenceNodeNextNodeValue(dwCurrentNodeIndex, true, dwCurrentNodeIndex + 1);
                        }
                    }
                }

                plugin.AddSequence(sequenceName, sequence);
            }

            return plugin;
        }
    }
}
