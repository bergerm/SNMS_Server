using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using SNMS_Server.Variables;
using SNMS_Server.RealTimeEngines;
using SNMS_Server.RealTimeEngines.Sequences;

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

                    XmlNode getItemSearchValueNode = commandNode.SelectSingleNode("SearchValue");
                    if (getItemSearchValueNode == null)
                    {
                        errorString = "Invalid Get Item Search Value";
                        return false;
                    }

                    if (getItemSearchValueNode.HasChildNodes && getItemSearchValueNode.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sGetItemSearchValue = getItemSearchValueNode.ChildNodes[0].InnerText.ToLower();
                        bGetItemSearchValueVariable = true;
                        if (plugin.GetVariable(sGetItemSearchValue) == null)
                        {
                            errorString = "Invalid Get Item Search Value";
                            return false;
                        }
                    }
                    else
                    {
                        sGetItemSearchValue = getItemSearchValueNode.InnerText;
                        bIsCommandSource2Variable = false;
                    }


                    if (sGetItemName == "" || sGetItemSearchType == "" || sGetItemSearchValue == "")
                    {
                        errorString = "Invalid Get Item name \"" + sGetItemName;
                        return false;
                    }

                    if (sGetItemParentElement != "" && plugin.WebElementExists(sGetItemParentElement) == false)
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
                        sequence.AddCommand(getItemByID, isConditionalNode);
                    }
                    else if (sGetItemSearchType == "xpath")
                    {
                        GetElementByXPathWebDriverCommand getItemByXPath = new GetElementByXPathWebDriverCommand(sGetItemParentElement,
                                                                                                            sGetItemName,
                                                                                                            sGetItemSearchValue,
                                                                                                            bGetItemSearchValueVariable);
                        sequence.AddCommand(getItemByXPath, isConditionalNode);
                    }
                    else if (sGetItemSearchType == "class")
                    {
                        GetElementByClassNameWebDriverCommand getItemByClass = new GetElementByClassNameWebDriverCommand(sGetItemParentElement,
                                                                                                            sGetItemName,
                                                                                                            sGetItemSearchValue,
                                                                                                            bGetItemSearchValueVariable);
                        sequence.AddCommand(getItemByClass, isConditionalNode);
                    }
                    else if (sGetItemSearchType == "class")
                    {
                        GetElementByCssSelectorWebDriverCommand getItemByCss = new GetElementByCssSelectorWebDriverCommand(sGetItemParentElement,
                                                                                                            sGetItemName,
                                                                                                            sGetItemSearchValue,
                                                                                                            bGetItemSearchValueVariable);
                        sequence.AddCommand(getItemByCss, isConditionalNode);
                    }
                    else if (sGetItemSearchType == "tag")
                    {
                        GetElementByTagNameWebDriverCommand getItemByTag = new GetElementByTagNameWebDriverCommand(sGetItemParentElement,
                                                                                                            sGetItemName,
                                                                                                            sGetItemSearchValue,
                                                                                                            bGetItemSearchValueVariable);
                        sequence.AddCommand(getItemByTag, isConditionalNode);
                    }
                    plugin.SetWebElement(sGetItemName, null);
                    break;

                case "GetActiveItem":
                    string sGetActiveItemDestination = commandNode.Attributes["name"].Value.ToLower();
                    GetActiveElementWebDriverCommand getActiveElementCommand = new GetActiveElementWebDriverCommand(sGetActiveItemDestination);
                    sequence.AddCommand(getActiveElementCommand);
                    break;
                    
                case "CheckItemVisible":
                    string sCheckElementVisibleName = commandNode.SelectSingleNode("ItemName").InnerText;
                    CheckElementVisibleWebDriverCommand checkElementVisibleCommand = new CheckElementVisibleWebDriverCommand(sCheckElementVisibleName);
                    sequence.AddCommand(checkElementVisibleCommand, isConditionalNode);
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
                    string sSetVariableName = commandNode.SelectSingleNode("Destination").InnerText.ToLower();
                    if (plugin.GetVariable(sSetVariableName) == null)
                    {
                        errorString = "Invalid SetVariable Destination";
                        return false;
                    }
                    string sSetVariableValue = commandNode.SelectSingleNode("String").InnerText;
                    SetVariableCommand setVariableCommand = new SetVariableCommand(sSetVariableName, sSetVariableValue);
                    sequence.AddCommand(setVariableCommand);
                    break;

                case "CompareVariables":
                    string sCompareVariable1Name = commandNode.SelectSingleNode("Variable1").InnerText.ToLower();
                    if (plugin.GetVariable(sCompareVariable1Name) == null)
                    {
                        errorString = "Invalid CompareVariables Source1";
                        return false;
                    }
                    string sCompareVariable2Name = commandNode.SelectSingleNode("Variable2").InnerText.ToLower();
                    if (plugin.GetVariable(sCompareVariable2Name) == null)
                    {
                        errorString = "Invalid CompareVariables Source2";
                        return false;
                    }
                    CompareVariableCommand compareVariableCommand = new CompareVariableCommand(sCompareVariable1Name, sCompareVariable2Name);
                    sequence.AddCommand(compareVariableCommand, isConditionalNode);
                    break;

                case "IncreaseVariable":
                    string sIncreaseVariableName = commandNode.SelectSingleNode("Destination").InnerText.ToLower();
                    if (plugin.GetVariable(sIncreaseVariableName) == null)
                    {
                        errorString = "Invalid IncreaseVariable Destination";
                        return false;
                    }
                    int dwIncreaseVariableValue = Int32.Parse(commandNode.SelectSingleNode("Quantity").InnerText);
                    IncreaseVariableCommand increaseVariableCommand = new IncreaseVariableCommand(sIncreaseVariableName, dwIncreaseVariableValue);
                    sequence.AddCommand(increaseVariableCommand);
                    break;

                case "DecreaseVariable":
                    string sDecreaseVariableName = commandNode.SelectSingleNode("Destination").InnerText.ToLower();
                    if (plugin.GetVariable(sDecreaseVariableName) == null)
                    {
                        errorString = "Invalid DecreaseVariable Destination";
                        return false;
                    }
                    int dwDecreaseVariableValue = Int32.Parse(commandNode.SelectSingleNode("Quantity").InnerText);
                    DecreaseVariableCommand decreaseVariableCommand = new DecreaseVariableCommand(sDecreaseVariableName, dwDecreaseVariableValue);
                    sequence.AddCommand(decreaseVariableCommand);
                    break;

                case "Call":
                    string callSequenceName = commandNode.InnerText;
                    CallCommand callCommand = new CallCommand(callSequenceName);
                    sequence.AddCommand(callCommand);
                    break;

                case "GetTextFromWebItem":
                    string sGetTextSource = commandNode.SelectSingleNode("Source").InnerText.ToLower();
                    if (plugin.WebElementExists(sGetTextSource) == false)
                    {
                        errorString = "Invalid GetTextFromWebItem element";
                        return false;
                    }
                    string sGetTextDest = commandNode.SelectSingleNode("Destination").InnerText.ToLower();
                    if (plugin.GetVariable(sGetTextDest) == null)
                    {
                        errorString = "Invalid GetTextFromWebItem Destination";
                        return false;
                    }
                    GetInnerTextFromElementWebDriverCommand getTextFromElementCommand = new GetInnerTextFromElementWebDriverCommand(sGetTextSource, sGetTextDest);
                    sequence.AddCommand(getTextFromElementCommand);
                    break;

                case "GetIntegerFromWebItem":
                    string sGetIntSource = commandNode.SelectSingleNode("Source").InnerText.ToLower();
                    if (plugin.WebElementExists(sGetIntSource) == false)
                    {
                        errorString = "Invalid GetIntegerFromWebItem element";
                        return false;
                    }
                    string sGetIntDest = commandNode.SelectSingleNode("Destination").InnerText.ToLower();
                    if (plugin.GetVariable(sGetIntSource) == null)
                    {
                        errorString = "Invalid GetIntegerFromWebItem Destination";
                        return false;
                    }
                    GetInnerIntegerFromElementWebDriverCommand getIntFromElementCommand = new GetInnerIntegerFromElementWebDriverCommand(sGetIntSource, sGetIntDest);
                    sequence.AddCommand(getIntFromElementCommand);
                    break;

                case "GetLinkFromWebItem":
                    string sGetLinkSource = commandNode.SelectSingleNode("Source").InnerText.ToLower();
                    if (plugin.WebElementExists(sGetLinkSource) == false)
                    {
                        errorString = "Invalid GetLinkFromWebItem element";
                        return false;
                    }
                    string sGetLinkDest = commandNode.SelectSingleNode("Destination").InnerText.ToLower();
                    if (plugin.GetVariable(sGetLinkDest) == null)
                    {
                        errorString = "Invalid GetLinkFromWebItem Destination";
                        return false;
                    }
                    GetInnerLinkFromElementWebDriverCommand getLinkFromElementCommand = new GetInnerLinkFromElementWebDriverCommand(sGetLinkSource, sGetLinkDest);
                    sequence.AddCommand(getLinkFromElementCommand);
                    break;

                case "GreaterThan":
                    XmlNode greaterThanSource1Node = commandNode.SelectSingleNode("Source1");
                    string sGreaterThanSource1 = "";
                    bool bGreaterThanSource1Variable = false;
                    if (greaterThanSource1Node.HasChildNodes && greaterThanSource1Node.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sGreaterThanSource1 = greaterThanSource1Node.InnerText.ToLower();
                        bGreaterThanSource1Variable = true;
                        if (plugin.GetVariable(sGreaterThanSource1) == null)
                        {
                            errorString = "Invalid Greater Than Command Variable";
                            return false;
                        }
                    }
                    else
                    {
                        sGreaterThanSource1 = greaterThanSource1Node.InnerText;
                        bGreaterThanSource1Variable = false;
                    }

                    XmlNode greaterThanSource2Node = commandNode.SelectSingleNode("Source2");
                    string sGreaterThanSource2 = "";
                    bool bGreaterThanSource2Variable = false;
                    if (greaterThanSource1Node.HasChildNodes && greaterThanSource1Node.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sGreaterThanSource2 = greaterThanSource2Node.InnerText.ToLower();
                        bGreaterThanSource2Variable = true;
                        if (plugin.GetVariable(sGreaterThanSource2) == null)
                        {
                            errorString = "Invalid Greater Than Command Variable";
                            return false;
                        }
                    }
                    else
                    {
                        sGreaterThanSource2 = greaterThanSource2Node.InnerText;
                        bGreaterThanSource2Variable = false;
                    }

                    GreaterThanCommand greaterThanCommand = new GreaterThanCommand(sGreaterThanSource1, bGreaterThanSource1Variable, sGreaterThanSource2, bGreaterThanSource2Variable);
                    sequence.AddCommand(greaterThanCommand, isConditionalNode);
                    break;

                case "VariableContains":
                    string sVariableContainsName = commandNode.SelectSingleNode("VariableName").InnerText.ToLower();
                    XmlNode variableContainsSourceNode = commandNode.SelectSingleNode("Source");
                    string sVariableContainsSource = "";
                    bool bVariableContainsSourceVariable = false;
                    if (variableContainsSourceNode.HasChildNodes && variableContainsSourceNode.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sVariableContainsSource = variableContainsSourceNode.InnerText.ToLower();
                        bVariableContainsSourceVariable = true;
                        if (plugin.GetVariable(sVariableContainsSource) == null)
                        {
                            errorString = "Invalid Greater Than Command Variable";
                            return false;
                        }
                    }
                    else
                    {
                        sVariableContainsSource = variableContainsSourceNode.InnerText;
                        bVariableContainsSourceVariable = false;
                    }
                    VariableContainsCommand variableContainsCommand = new VariableContainsCommand(sVariableContainsName,
                                                                                                    sVariableContainsSource,
                                                                                                    bVariableContainsSourceVariable);
                    sequence.AddCommand(variableContainsCommand, isConditionalNode);
                    break;

                case "CheckTriggers":
                    string sTriggersType = commandNode.SelectSingleNode("TriggerType").InnerText;
                    string sVariableName = commandNode.SelectSingleNode("VariableName").InnerText;
                    string sReaction = commandNode.SelectSingleNode("Reaction").InnerText;
                    CheckTriggersCommand checkTriggerCommand = new CheckTriggersCommand(sTriggersType, sVariableName, sReaction);
                    sequence.AddCommand(checkTriggerCommand, isConditionalNode);
                    break;

                case "LogLink":
                    XmlNode linkMessageSource = commandNode.SelectSingleNode("Message");
                    bool bIsLinkMessageSourceVariable = false;
                    string sLinkMessageSource = "";
                    if (linkMessageSource.HasChildNodes && linkMessageSource.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sLinkMessageSource = linkMessageSource.ChildNodes[0].InnerText.ToLower();
                        bIsLinkMessageSourceVariable = true;
                        if (plugin.GetVariable(sLinkMessageSource) == null)
                        {
                            errorString = "Invalid LogLink Message";
                            return false;
                        }
                    }
                    else
                    {
                        sLinkMessageSource = linkMessageSource.InnerText;
                        bIsLinkMessageSourceVariable = false;
                    }

                    XmlNode linkSource = commandNode.SelectSingleNode("Link");
                    bool bIsLinkSourceVariable = false;
                    string sLinkSource = "";
                    if (linkSource.HasChildNodes && linkSource.ChildNodes[0].NodeType != XmlNodeType.Text)
                    {
                        sLinkSource = linkSource.ChildNodes[0].InnerText.ToLower();
                        bIsLinkSourceVariable = true;
                        if (plugin.GetVariable(sLinkSource) == null)
                        {
                            errorString = "Invalid LogLink Message";
                            return false;
                        }
                    }
                    else
                    {
                        sLinkSource = linkMessageSource.InnerText;
                        bIsLinkSourceVariable = false;
                    }

                    LogLinkCommand logLinkCommand = new LogLinkCommand( sLinkMessageSource,
                                                                        bIsLinkMessageSourceVariable,
                                                                        sLinkSource,
                                                                        bIsLinkSourceVariable);
                    sequence.AddCommand(logLinkCommand);
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
                string varValue = variableNode.SelectSingleNode("ConstantValue").InnerText;

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
                                                    plugin.GetWebDriver(),
                                                    true);

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

                            case "jump":
                                trueConditionDetinationLabelDictionary.Add(Math.Max(dwCurrentNodeIndex-1,0), commandNode.InnerText);
                                falseConditionDetinationLabelDictionary.Add(Math.Max(dwCurrentNodeIndex-1,0), commandNode.InnerText);
                                bIsCurrentNodeConditional = false;
                                break;

                            default:
                                if (!HandleCommand(plugin, sequence, sequenceName, commandNode, bIsCurrentNodeConditional, ref errorString))
                                {
                                    errorString += "; Error in plugin " + plugin.GetPluginName() + ", sequence " + sequenceName + ", index " + dwCurrentNodeIndex + " (" +
                                        sCommandName + ")";
                                    return null;
                                }
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

            // Get Startup Sequences
            XmlNode pluginStartupSequences = pluginNode.SelectSingleNode("Start");
            XmlNodeList sequences = pluginStartupSequences.SelectNodes("Sequence");
            foreach (XmlNode sequenceNode in sequences)
            {
                string sSequenceName = sequenceNode.InnerText;
                plugin.AddStartupSequence(sSequenceName);
            }

            // Get Timers
            XmlNodeList pluginTimers = pluginNode.SelectNodes("Timer");
            foreach (XmlNode timerNode in pluginTimers)
            {
                SequenceTimer timer = new SequenceTimer();
                timer.SetSequenceName(timerNode.SelectSingleNode("SequenceName").InnerText);
                timer.SetPeriod(Int32.Parse(timerNode.SelectSingleNode("Period").InnerText));

                plugin.AddTimer(timer);
            }

            return plugin;
        }
    }
}
