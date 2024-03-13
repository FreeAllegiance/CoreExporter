using System;
using System.Collections.Generic;
using System.Text;
using FreeAllegiance.IGCCore;
using FreeAllegiance.IGCCore.Modules;
using System.Diagnostics;
using System.Collections;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CoreExporter
{
    public class TechTree
    {
        private enum GlobalDefinitions
        {
            AllowExpansion = 5,
            AllowShipyard = 6,
            AllowSupremacy = 3,
            AllowTacticalLab = 4
        }

        private readonly List<int> _globalDefinitions = new List<int>(new int[] { (int)GlobalDefinitions.AllowExpansion, (int)GlobalDefinitions.AllowShipyard, (int)GlobalDefinitions.AllowSupremacy, (int)GlobalDefinitions.AllowTacticalLab });
        private Core _core;

        private Node _rootNode = null;
        public Node RootNode
        {
            get { return _rootNode; }
        }

        private List<Node> _factionNodes = new List<Node>();
        public List<Node> FactionNodes
        {
            get { return _factionNodes; }
        }

        public void CreateSampleTree()
        {
            _rootNode = new Node(null, "Root");

            Node child1 = new Node(_rootNode, "child 1");
            Node child2 = new Node(_rootNode, "child 2");
            //Node child3 = new Node(_rootNode, "child 3");
            //Node child4 = new Node(_rootNode, "child 4");

            Node child11 = new Node(child1, "child 1 1");
            Node child111 = new Node(child11, "child 1 1 1");
            Node child1111 = new Node(child111, "child 1 1 1 1");
            Node child11111 = new Node(child1111, "child 1 1 1 1 1");
            Node child21 = new Node(child2, "child 2 1");
            Node child211 = new Node(child21, "child 2 1 1");
            Node child2111 = new Node(child211, "child 2 1 1 1");
            Node child21111 = new Node(child2111, "child 2 1 1 1 1");



            child21111.SetParent(child11);
            child21111.SetParent(child111);

            //Node child12 = new Node(child1, "child 1 2");
            //Node child13 = new Node(child1, "child 1 3");

            //Node child111 = new Node(child11, "child 1 1 1");
            //Node child112 = new Node(child11, "child 1 1 2");
            //Node child1111 = new Node(child111, "child 1 1 1 1");
            //Node child11111 = new Node(child1111, "child 1 1 1 1 1");
            //Node child11112 = new Node(child1111, "child 1 1 1 1 2");
            //Node child11113 = new Node(child1111, "child 1 1 1 1 3");
            //Node child11114 = new Node(child1111, "child 1 1 1 1 4");

            //Node child111141 = new Node(child11114, "child 1 1 1 1 4 1");
            //Node child111142 = new Node(child11114, "child 1 1 1 1 4 2");



            //Node child31 = new Node(child3, "child 3 1");
            //Node child32 = new Node(child3, "child 3 2");
            //Node child33 = new Node(child3, "child 3 3");

            //Node child321 = new Node(child32, "child 3 2 1");
            //Node child322 = new Node(child32, "child 3 2 2");
            //Node child323 = new Node(child32, "child 3 2 3");


            //Node child121 = new Node(child12, "child 1 2 1");
            //Node child122 = new Node(child12, "child 1 2 2");
            //Node child123 = new Node(child12, "child 1 2 3");
            //Node child124 = new Node(child12, "child 1 2 4");

            //child11112.SetParent(child122);
            //child11113.SetParent(child123);
            //child11114.SetParent(child124);

            //child111.SetParent(child322);
            //child112.SetParent(child322);

            //Node child1211 = new Node(child121, "child 1 2 1 1");
            //Node child12111 = new Node(child1211, "child 1 2 1 1 1");

            //child11114.SetParent(child12111);

            //Node child21 = new Node(child2, "child 2 1");

            //Node child211 = new Node(child2, "child 2 1 1");

            //child11.SetParent(child211);
            //child12.SetParent(child211);

            //child21.SetParent(child121);

            //child32.SetParent(child21);
            //child11.SetParent(child21);

            //for (int i = 0; i < 1; i++)
            //{
            //    Node child4x = new Node(child4, "Child 4 " + i);
            //    child1211.SetParent(child4x);
            //}
        }

        public void LoadCoreFile(string filename)
        {
            _core = new Core(filename);

            _rootNode = null;
            _factionNodes.Clear();

            foreach (IGCCoreCiv civ in _core.Factions)
            {
                //System.Diagnostics.Debug.WriteLine("civ: " + civ.Name + ", defs: " + civ.Techtree.GetDefs() + ", pres: " + civ.Techtree.GetPres());

                //if (civ.Name == targetCivName)
                //{
                //    _rootNode = new Node(civ);
                //}

                _factionNodes.Add(new Node(civ));
            }

            //if (_rootNode == null)
            //    throw new Exception("Couldn't find target civilization name with the name of: " + targetCivName);

            //BuildNodeTreeFromCore(core, _rootNode);
        }


        private void ClearAllTreeConnections(Node nodeToClear)
        {
            if (nodeToClear == null)
                return;

            foreach (Node childNode in nodeToClear.Children)
            {
                ClearAllTreeConnections(childNode);
            }

            nodeToClear.Children.Clear();
            nodeToClear.Parents.Clear();
        }


        public void BuildFactionNodeTreeFromCore(Node factionNode)
        {
            ClearAllTreeConnections(_rootNode);

            _rootNode = factionNode;
            List<Node> linearNodeList = new List<Node>();
            linearNodeList.Add(factionNode);

            foreach (IGCCoreStationType station in _core.StationTypes)
            {
                //Debug.WriteLine("station: " + station.Name + ", pres: " + station.Techtree.GetPres());
                //Debug.WriteLine("station: " + station.Name + ", local: " + ParseLocalTechTreeToString(station.TechTreeLocal));

                linearNodeList.Add(new Node(station));
            }


            foreach (IGCCoreShip ship in _core.Ships)
            {
                linearNodeList.Add(new Node(ship));
            }

            foreach (IGCCoreDevel devel in _core.Developments)
            {
                linearNodeList.Add(new Node(devel));
            }

            SatisfyAllPrerequisites(linearNodeList, factionNode);
            Node[] temporaryNodeList = new Node[linearNodeList.Count];
            linearNodeList.CopyTo(temporaryNodeList);

            // Remove all nodes that are not available to the target faction.
            foreach (Node node in temporaryNodeList)
            {
                if (node.CoreObject.UID == 37)
                    Debug.Write("");


                if (node.HasSatisfiedAllPrerequisites == false && node != factionNode)
                    linearNodeList.Remove(node);
            }



            // Build a node list of all stations.
            List<Node> stationNodes = new List<Node>();
            List<Node> shipNodes = new List<Node>();
            foreach (Node node in linearNodeList)
            {
                if (node.CoreObject is IGCCoreStationType)
                    stationNodes.Add(node);

                if (node.CoreObject is IGCCoreShip)
                    shipNodes.Add(node);
            }

            // Build a list of stations that don't require any development. 
            List<Node> rootLevelStationNodes = FindRootLevelStationNodes(stationNodes, _core, (IGCCoreCiv)factionNode.CoreObject);

            // Build a list of stations that require development.
            List<Node> developedStationNodes = new List<Node>();
            foreach (Node stationNode in stationNodes)
            {
                if (rootLevelStationNodes.Contains(stationNode) == false)
                    developedStationNodes.Add(stationNode);
            }

            // Add all the root level stations to the root node.
            foreach (Node rootLevelStationNode in rootLevelStationNodes)
            {
                if (factionNode.AreNodesRelated(rootLevelStationNode) == false)
                    rootLevelStationNode.SetParent(factionNode);
            }

            List<Node> developmentNodes = new List<Node>();
            foreach (Node node in linearNodeList)
            {
                if (node.CoreObject is IGCCoreDevel)
                    developmentNodes.Add(node);
            }

            // Anchor all the root level development tech into it's associated station branch. 
            // devel tech is not directly linked to stations, it must be mapped based to a 
            // station type based on it's tech tree path, and then assigned to a station level (standard, advanced, etc...)
            // based on the prereqs for the station. 
            foreach (Node rootStation in rootLevelStationNodes)
            {
                foreach (Node development in developmentNodes)
                {
                    if (IsDevelopmentInStationTechBranch(rootStation, development) == true)
                    {


                        bool isRootLevelDevelopment = true;

                        foreach (int prerequisiteID in development.Prerequisites)
                        {
                            if (development.CoreObject.UID == 47 && prerequisiteID == 138)
                                Debug.Write("");

                            if (rootStation.Definitions.Contains(prerequisiteID) == true)
                                continue;

                            // For factions, pre is def, and def is 'no devel' defaults.
                            if (factionNode.Prerequisites.Contains(prerequisiteID) == true)
                                continue;

                            if (_globalDefinitions.Contains(prerequisiteID) == true)
                                continue;

                            bool prerequisiteWasDefinedByAnotherRootStation = false;
                            foreach (Node otherRootStation in rootLevelStationNodes)
                            {
                                if (otherRootStation == rootStation)
                                    continue;

                                if (otherRootStation.Definitions.Contains(prerequisiteID) == true)
                                {
                                    prerequisiteWasDefinedByAnotherRootStation = true;
                                    break;
                                }
                            }

                            if (prerequisiteWasDefinedByAnotherRootStation == true)
                                continue;

                            // If the prerequisite is defined by another development in this tech path, then 
                            // this development is not a root development. 
                            bool prerequisiteWasDefinedByAnotherDevelopmentInThisBranch = false;
                            foreach (Node nodeToTest in developmentNodes)
                            {
                                //if (development.CoreObject.UID == 73 && nodeToTest.CoreObject.UID == 74)
                                //    Debugger.Break();

                                if (nodeToTest.Definitions.Contains(prerequisiteID) == true)
                                {
                                    if (((IGCCoreDevel)nodeToTest.CoreObject).Root_Tree == ((IGCCoreDevel)development.CoreObject).Root_Tree)
                                    {
                                        prerequisiteWasDefinedByAnotherDevelopmentInThisBranch = true;
                                        break;
                                    }
                                }
                            }

                            if (prerequisiteWasDefinedByAnotherDevelopmentInThisBranch == true)
                            {
                                isRootLevelDevelopment = false;
                                break;
                            }

                            // If the prerequisite needs a non-root station, then it is not a root development. 
                            bool prerequisiteWasDefinedByNonRootStation = false;
                            foreach (Node developedStation in developedStationNodes)
                            {
                                if (developedStation.Definitions.Contains(prerequisiteID) == true)
                                {
                                    prerequisiteWasDefinedByNonRootStation = true;
                                    break;
                                }
                            }

                            if (prerequisiteWasDefinedByNonRootStation == true)
                            {
                                isRootLevelDevelopment = false;
                                break;
                            }

                            isRootLevelDevelopment = false;
                            break;
                        }

                        if (isRootLevelDevelopment == true && development.AreNodesRelated(rootStation) == false)
                            development.SetParent(rootStation);
                    }
                }
            }


            // Add stations that need development to the tree.
            foreach (Node station in developedStationNodes)
            {
                List<Node> nodesThatDependOnStation = new List<Node>();

                foreach (Node development in developmentNodes)
                {
                    if (development.Defines(station, false) == true && development.AreNodesRelated(station) == false)
                        station.SetParent(development);

                    /*
					// Attach all developments that need a developed station and are not attached to another development in 
					// this station's tech branch.
					if (station.Defines(development, false) == true && station.AreNodesRelated(development) == false && development.Parents.Count == 0 && IsDevelopmentInStationTechBranch(station, development) == true)
					{
						bool isDevelopmentDefinedByRootLevelStation = false;
						//foreach (Node rootStation in rootLevelStationNodes)
						//{
						//    if (rootStation.Defines(development, false) == true && rootStation.AreNodesRelated(development) == false)
						//    {
						//        isDevelopmentDefinedByRootLevelStation = true;
						//        development.SetParent(rootStation);
						//        break;
						//    }
						//}

						if (isDevelopmentDefinedByRootLevelStation == false)
							nodesThatDependOnStation.Add(development);
					}
					*/



                    //bool isDevelopmentDefinedByRootLevelStation = false;
                    //foreach (Node rootStation in rootLevelStationNodes)
                    //{
                    //    if (rootStation.Defines(development, false) == true)
                    //    {
                    //        if (rootStation.AreNodesRelated(development) == false)
                    //            development.SetParent(rootStation);
                    //        isDevelopmentDefinedByRootLevelStation = true;
                    //        break;
                    //    }
                    //}

                    //if (isDevelopmentDefinedByRootLevelStation == false)
                    //{
                    //    // Attach all developments that need a developed station and are not attached to another development in 
                    //    // this station's tech branch.
                    //    if (station.Defines(development, false) == true && station.AreNodesRelated(development) == false && development.Parents.Count == 0 && IsDevelopmentInStationTechBranch(station, development) == true)
                    //    {
                    //        nodesThatDependOnStation.Add(development);
                    //    }
                    //}


                }

                //foreach (Node parentNode in nodesThatDependOnStation)
                //{
                //    foreach (Node childNode in nodesThatDependOnStation)
                //    {
                //        if (parentNode == childNode)
                //            continue;

                //        if (parentNode.Defines(childNode, false) && parentNode.AreNodesRelated(childNode) == false)
                //        {
                //            childNode.SetParent(parentNode);
                //            break;
                //        }
                //    }
                //}

                //foreach (Node node in nodesThatDependOnStation)
                //{
                //    if (node.Parents.Count == 0)
                //        node.SetParent(station);
                //}
            }


            // Find all developments that define stations.
            List<Node> developmentsThatDefineStations = new List<Node>();
            foreach (Node development in developmentNodes)
            {
                foreach (Node station in stationNodes)
                {
                    if (development.Defines(station, false) == true)
                        developmentsThatDefineStations.Add(development);
                }
            }

            // Determine proper linking for developments that define stations.
            //foreach (Node parentDevelopment in developmentsThatDefineStations)
            //{
            //    foreach (Node childDevelopment in developmentsThatDefineStations)
            //    {
            //        if (childDevelopment == parentDevelopment)
            //            continue; 

            //        if (childDevelopment.Parents.Count == 0 && parentDevelopment.Defines(childDevelopment, false) == true && parentDevelopment.AreNodesRelated(childDevelopment) == false)
            //            childDevelopment.SetParent(parentDevelopment);
            //    }
            //}

            // Connect development nodes with stations as the primary parent.
            foreach (Node development in developmentNodes)
            {
                if (development.CoreObject.UID == 47)
                    Debug.Write("");

                if (development.Parents.Count == 0)
                {
                    Node station = FindLowestLevelStationThatDefinesDevelopment(_rootNode, null, development);

                    // Does this development rely on any other development in this branch?
                    foreach (Node parentDevelopmentCandidate in developmentNodes)
                    {
                        if (parentDevelopmentCandidate == development)
                            continue;

                        // If the parent development defined this development, and the parent development is defined by the same station level that defines the development, 
                        // then the proper parent development has been found.
                        if (parentDevelopmentCandidate.Defines(development, false) && FindLowestLevelStationThatDefinesDevelopment(_rootNode, null, parentDevelopmentCandidate) == station && parentDevelopmentCandidate.AreNodesRelated(development) == false)
                        {
                            development.SetParent(parentDevelopmentCandidate);
                            break;
                        }
                    }

                    // No parent developments were found in this branch, so add the development to the root station.
                    if (development.Parents.Count == 0 && station != null && station.AreNodesRelated(development) == false)
                        development.SetParent(station);
                }
            }

            // Interconnect developments that are not connected to anything. 
            foreach (Node development in developmentNodes)
            {
                //if (development.CoreObject.UID == 224)
                //    Debug.Write("");

                if (development.Parents.Count > 0)
                    continue;

                foreach (Node parentDevelopment in developmentNodes)
                {
                    if (parentDevelopment == development)
                        continue;

                    if (parentDevelopment.Defines(development, false) && parentDevelopment.AreNodesRelated(development) == false)
                    {
                        development.SetParent(parentDevelopment);
                        break;
                    }
                }
            }

            // Cross Connect dependent developments
            foreach (Node development in developmentNodes)
            {
                foreach (Node parentDevelopment in developmentNodes)
                {
                    if (parentDevelopment == development)
                        continue;

                    if (parentDevelopment.Defines(development, false) == true && development.AreNodesRelated(parentDevelopment) == false)
                        development.SetParent(parentDevelopment);
                }
            }

            // Attach Ships to the tech tree
            foreach (Node ship in shipNodes)
            {
                if (ship.CoreObject.UID == 37)
                    Debug.Write("");

                foreach (Node development in developmentNodes)
                {
                    if (development.Defines(ship, false) == true && ship.AreNodesRelated(development) == false)
                        ship.SetParent(development);
                }

                foreach (int prerequisiteID in ship.Prerequisites)
                {
                    Node lowestLevelStation = null;
                    foreach (Node station in stationNodes)
                    {
                        if (station.Definitions.Contains(prerequisiteID))
                        {
                            if (lowestLevelStation == null || station.Depth < lowestLevelStation.Depth)
                                lowestLevelStation = station;
                        }
                    }

                    if (lowestLevelStation != null && ship.AreNodesRelated(lowestLevelStation) == false)
                        ship.SetParent(lowestLevelStation);
                }

                /*
				List<Node> usedDevelopedStationNodes = new List<Node>();
				foreach (Node developedStation in developedStationNodes)
				{
					if (developedStation.Defines(ship, false) == true && developedStation.AreNodesRelated(ship) == false)
					{
						usedDevelopedStationNodes.Add(developedStation);
						ship.SetParent(developedStation);
					}
				}

				//List<Node> usedRootStationNodes = new List<Node>();
				foreach (Node rootStation in rootLevelStationNodes)
				{
					if (rootStation.Defines(ship, false) == true && rootStation.AreNodesRelated(ship) == false)
					{

						bool isShipAlreadyAssignedToDevelopedStationUnderThisRoot = false;
						foreach (Node developedStation in usedDevelopedStationNodes)
						{
							if (rootStation.IsNodeADecendant(developedStation) == true)
							{
								isShipAlreadyAssignedToDevelopedStationUnderThisRoot = true;
								break;
							}
						}

						if(isShipAlreadyAssignedToDevelopedStationUnderThisRoot == false)
							ship.SetParent(rootStation);
					}
				}
				*/

            }

            CombNodeAttachmentLevels(_rootNode);

            ClearRedunantLinks(_rootNode);



        }

        /// <summary>
        /// If any nodes are attached to development nodes that lower than another development node higher in the tree, move the node up to 
        /// to the proper level in the tree. 
        /// </summary>
        /// <param name="_rootNode"></param>
        private void CombNodeAttachmentLevels(Node nodeToComb)
        {
            if (nodeToComb.CoreObject.UID == 19)
                Debug.Write("");

            bool movedNode = false;
            do
            {
                movedNode = false;

                if (nodeToComb.Parents.Count > 0)
                {
                    List<int> definingPrerequisites = GetDefiningPrerequisite(nodeToComb.Parents[0], nodeToComb);

                    // Find next available ancestor that also defines the attachment prerequisistes. 
                    Node ancestorThatAlseDefinesPrerequistes = GetParentNodeThatDefinesPrerequisites(nodeToComb.Parents[0], definingPrerequisites);

                    if (ancestorThatAlseDefinesPrerequistes != null)
                    {
                        // Break the link to the node's primary parent.
                        nodeToComb.Parents[0].Children.Remove(nodeToComb);
                        nodeToComb.Parents.RemoveAt(0);

                        // Make a new link to the node's up level ancestor.
                        nodeToComb.Parents.Insert(0, ancestorThatAlseDefinesPrerequistes);
                        ancestorThatAlseDefinesPrerequistes.Children.Add(nodeToComb);

                        movedNode = true;
                    }
                }


            } while (movedNode == true);

            List<Node> childNodes = new List<Node>(nodeToComb.Children);
            foreach (Node childNode in childNodes)
                CombNodeAttachmentLevels(childNode);
        }

        private List<int> GetDefiningPrerequisite(Node parentNode, Node childNode)
        {
            List<int> definingPrerequisites = new List<int>();
            foreach (int prerequisiteID in childNode.Prerequisites)
            {
                foreach (int definitionID in parentNode.Definitions)
                {
                    if (prerequisiteID == definitionID)
                        definingPrerequisites.Add(prerequisiteID);
                }

                foreach (int definitionID in parentNode.Locals)
                {
                    if (prerequisiteID == definitionID)
                        definingPrerequisites.Add(prerequisiteID);
                }
            }

            return definingPrerequisites;
        }

        private Node GetParentNodeThatDefinesPrerequisites(Node node, List<int> definingPrerequisites)
        {
            foreach (Node parentNode in node.Parents)
            {
                if (parentNode.Parents.Count == 0)
                    continue;

                int matchCount = 0;
                foreach (int definitionID in parentNode.Definitions)
                {
                    if (definingPrerequisites.Contains(definitionID) == true)
                        matchCount++;
                }

                foreach (int definitionID in parentNode.Locals)
                {
                    if (definingPrerequisites.Contains(definitionID) == true)
                        matchCount++;
                }

                if (matchCount >= definingPrerequisites.Count)
                    return parentNode;
            }

            Node nearestParentNode = null;
            foreach (Node parentNode in node.Parents)
            {
                Node parentNodeCandidate = GetParentNodeThatDefinesPrerequisites(parentNode, definingPrerequisites);
                if (nearestParentNode == null || (parentNodeCandidate != null && parentNodeCandidate.Depth > nearestParentNode.Depth))
                    nearestParentNode = parentNodeCandidate;
            }

            return nearestParentNode;
        }
        private Node FindLowestLevelStationThatDefinesDevelopment(Node nodeToTest, Node bestStationNode, Node development)
        {
            if (development.CoreObject.UID == 255 && nodeToTest.CoreObject.UID == 72)
                Debug.Write("");

            if (nodeToTest.CoreObject is IGCCoreStationType && nodeToTest.Defines(development, false) == true && IsDevelopmentInStationTechBranch(nodeToTest, development) == true)
            {
                if (bestStationNode == null)
                {
                    bestStationNode = nodeToTest;
                }
                else
                {
                    if (nodeToTest.Depth < bestStationNode.Depth)
                        bestStationNode = nodeToTest;
                }
            }

            foreach (Node childNode in nodeToTest.Children)
                bestStationNode = FindLowestLevelStationThatDefinesDevelopment(childNode, bestStationNode, development);

            return bestStationNode;
        }



        //private bool IsDevelopmentLinkedToAnOrpanedDevelopment(Node development, List<Node> developmentNodes)
        //{
        //    foreach (Node parentDevelopment in developmentNodes)
        //    {
        //        if (parentDevelopment == development)
        //            continue;

        //        if(development.Parents.Contains(parentDevelopment) == false
        //    }
        //}

        //private void BuildNodeTreeFromCoreTry2(Core core, Node rootNode)
        //{


        //    // Build a node list of all stations.
        //    List<Node> stationNodes = new List<Node>();
        //    foreach (IGCCoreStationType station in core.StationTypes)
        //    {
        //        if(DoesFactionDefinePrerequisitesForObject(core, (IGCCoreCiv)rootNode.CoreObject, station) == true)
        //            stationNodes.Add(new Node(station));
        //    }

        //    // Build a list of stations that don't require any development. 
        //    List<Node> rootLevelStationNodes = FindRootLevelStationNodes(stationNodes, core, (IGCCoreCiv)rootNode.CoreObject);

        //    // Build a list of stations that require development.
        //    List<Node> developedStationNodes = new List<Node>();
        //    foreach (Node stationNode in stationNodes)
        //    {
        //        if (rootLevelStationNodes.Contains(stationNode) == false)
        //            developedStationNodes.Add(stationNode);
        //    }

        //    // Add all the root level stations to the root node.
        //    foreach (Node rootLevelStationNode in rootLevelStationNodes)
        //    {
        //        if (rootNode.AreNodesRelated(rootLevelStationNode) == false)
        //            rootLevelStationNode.SetParent(rootNode);
        //    }

        //    List<Node> developmentNodes = new List<Node>();
        //    foreach (IGCCoreDevel development in core.Developments)
        //    {
        //        if (DoesFactionDefinePrerequisitesForObject(core, (IGCCoreCiv)rootNode.CoreObject, development) == true)
        //            developmentNodes.Add(new Node(development));
        //    }

        //    // Anchor all the root level development tech into it's associated station branch. 
        //    // devel tech is not directly linked to stations, it must be mapped based to a 
        //    // station type based on it's tech tree path, and then assigned to a station level (standard, advanced, etc...)
        //    // based on the prereqs for the station. 
        //    foreach (Node rootStation in rootLevelStationNodes)
        //    {
        //        foreach (Node development in developmentNodes)
        //        {
        //            if (development.CoreObject.UID == 74)
        //                Debugger.Break();

        //            if (IsDevelopmentInStationTechBranch(rootStation, development) == true)
        //            {



        //                bool isRootLevelDevelopment = true;

        //                foreach (int prerequisiteID in development.Prerequisites)
        //                {
        //                    if (rootStation.Definitions.Contains(prerequisiteID) == true)
        //                        continue;

        //                    // For factions, pre is def, and def is 'no devel' defaults.
        //                    if (rootNode.Prerequisites.Contains(prerequisiteID) == true)
        //                        continue;

        //                    if (_globalDefinitions.Contains(prerequisiteID) == true)
        //                        continue;

        //                    bool prerequisiteWasDefinedByAnotherRootStation = false;
        //                    foreach (Node otherRootStation in rootLevelStationNodes)
        //                    {
        //                        if (otherRootStation == rootStation)
        //                            continue;

        //                        if (otherRootStation.Definitions.Contains(prerequisiteID) == true)
        //                        {
        //                            prerequisiteWasDefinedByAnotherRootStation = true;
        //                            break;
        //                        }
        //                    }

        //                    if (prerequisiteWasDefinedByAnotherRootStation == true)
        //                        continue;

        //                    // If the prerequisite is defined by another development in this tech path, then 
        //                    // this development is not a root development. 
        //                    bool prerequisiteWasDefinedByAnotherDevelopmentInThisBranch = false;
        //                    foreach (Node nodeToTest in developmentNodes)
        //                    {
        //                        if (development.CoreObject.UID == 73 && nodeToTest.CoreObject.UID == 74)
        //                            Debugger.Break();

        //                        if (nodeToTest.Definitions.Contains(prerequisiteID) == true)
        //                        {
        //                            if (((IGCCoreDevel)nodeToTest.CoreObject).Root_Tree == ((IGCCoreDevel)development.CoreObject).Root_Tree)
        //                            {
        //                                prerequisiteWasDefinedByAnotherDevelopmentInThisBranch = true;
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    if (prerequisiteWasDefinedByAnotherDevelopmentInThisBranch == false)
        //                        continue;

        //                    // If the prerequisite needs a non-root station, then it is not a root development. 
        //                    bool prerequisiteWasDefinedByNonRootStation = false;
        //                    foreach (Node developedStation in developedStationNodes)
        //                    {
        //                        if (developedStation.Definitions.Contains(prerequisiteID) == true)
        //                        {
        //                            prerequisiteWasDefinedByNonRootStation = true;
        //                            break;
        //                        }
        //                    }

        //                    if (prerequisiteWasDefinedByNonRootStation == false)
        //                        continue;

        //                    isRootLevelDevelopment = false;
        //                    break;
        //                }

        //                if (isRootLevelDevelopment == true)
        //                    development.SetParent(rootStation);
        //            }
        //        }
        //    }



        //    //bool atLeaseOneNodeAssignmentOccurred;
        //    //do
        //    //{
        //    //    atLeaseOneNodeAssignmentOccurred = false;




        //    //} while (atLeaseOneNodeAssignmentOccurred == true);



        //    /*
        //    // Assign all nodes with no development pre-reqs to the root. 
        //    foreach (Node topLevelCandidateNode in developmentNodeList)
        //    {
        //        bool foundDevelopmentPrerequisite = false;

        //        foreach (Node node in developmentNodeList)
        //        {
        //            if (node == topLevelCandidateNode)
        //                continue;

        //            foreach (int prerequisiteID in topLevelCandidateNode.Prerequisites)
        //            {
        //                if (node.Definitions.Contains(prerequisiteID) == true)
        //                {
        //                    foundDevelopmentPrerequisite = true;
        //                    break;
        //                }
        //            }

        //            if (foundDevelopmentPrerequisite == true)
        //                break;
        //        }

        //        if (foundDevelopmentPrerequisite == false)
        //        {
        //            if (topLevelCandidateNode.CoreObject is IGCCoreDevel)
        //            {
        //                AttachNodeToStation(topLevelCandidateNode, rootLevelStationNodes);
        //            }
        //            else
        //            {
        //                if (rootNode.AreNodesRelated(topLevelCandidateNode) == false)
        //                    topLevelCandidateNode.SetParent(rootNode);
        //            }
        //        }

        //    }
        //      */

        //    // Place all unassigned nodes into a list for assignment.
        //    //List<Node> unassignedNodes = new List<Node>();
        //    //foreach (Node node in developedStationNodes)
        //    //{
        //    //    if (node.Parents.Count == 0)
        //    //        unassignedNodes.Add(node);
        //    //}

        //    //foreach (Node node in developmentNodes)
        //    //{
        //    //    if (node.Parents.Count == 0)
        //    //        unassignedNodes.Add(node);
        //    //}

        //    //int lastCount = unassignedNodes.Count;
        //    //do
        //    //{
        //    //    lastCount = unassignedNodes.Count;

        //    //    foreach (Node node in unassignedNodes)
        //    //    {
        //    //        //if (parentNode.CoreObject.UID == 209)
        //    //        //    Debugger.Break();

        //    //        // Only attached nodes can be parent nodes.
        //    //        if (parentNode.Parents.Count == 0)
        //    //            continue;

        //    //        foreach (Node childNode in developmentNodes)
        //    //        {
        //    //            //if (parentNode.CoreObject.UID == 209 && childNode.CoreObject.UID == 210)
        //    //            //    Debugger.Break();

        //    //            if (childNode == parentNode)
        //    //                continue;

        //    //            foreach (int prerequisiteID in childNode.Prerequisites)
        //    //            {
        //    //                if (parentNode.Definitions.Contains(prerequisiteID) == true)
        //    //                {
        //    //                    if (parentNode.AreNodesRelated(childNode) == false)
        //    //                    {
        //    //                        childNode.SetParent(parentNode);
        //    //                        madeAtLeastOneAssignment = true;
        //    //                        break;
        //    //                    }
        //    //                }
        //    //            }
        //    //        }
        //    //    }

        //    //} while (unassignedNodes.Count > 0 && lastCount != unassignedNodes.Count);



        //    // Comb through the unassigned nodes until all are assigned.
        //    bool madeAtLeastOneAssignment = false;
        //    do
        //    {
        //        madeAtLeastOneAssignment = false;

        //        foreach (Node parentNode in developmentNodes)
        //        {
        //            //if (parentNode.CoreObject.UID == 209)
        //            //    Debugger.Break();

        //            // Only attached nodes can be parent nodes.
        //            if (parentNode.Parents.Count == 0)
        //                continue;

        //            foreach (Node childNode in developmentNodes)
        //            {
        //                //if (parentNode.CoreObject.UID == 209 && childNode.CoreObject.UID == 210)
        //                //    Debugger.Break();

        //                if (childNode == parentNode)
        //                    continue;

        //                // The first parent must be in the item's root tree.
        //                if (childNode.Parents.Count == 0 && ((IGCCoreDevel)childNode.CoreObject).Root_Tree != ((IGCCoreDevel)parentNode.CoreObject).Root_Tree)
        //                    continue;

        //                foreach (int prerequisiteID in childNode.Prerequisites)
        //                {
        //                    if (parentNode.Definitions.Contains(prerequisiteID) == true)
        //                    {
        //                        if (parentNode.AreNodesRelated(childNode) == false)
        //                        {
        //                            childNode.SetParent(parentNode);
        //                            madeAtLeastOneAssignment = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    while (madeAtLeastOneAssignment == true);


        //    // Remove links between objects which are dependant on another object when both share the same parent. 
        //    ClearRedunantLinks(rootNode);


        //}


        /// <summary>
        /// Remove links between objects which are dependant on another object when both share the same parent. 
        /// </summary>
        /// <param name="node"></param>
        private void ClearRedunantLinks(Node node)
        {
            if (node.CoreObject.UID == 236)
                Debug.Write("");
            List<Node> parentsToTest = new List<Node>(node.Parents);

            foreach (Node parent in parentsToTest)
            {
                if (node.Parents.Contains(parent) == false)
                    continue;

                if (node.Parents.Count <= 1)
                    break;

                foreach (Node otherParent in parentsToTest)
                {
                    if (parent == otherParent)
                        continue;

                    if (node.Parents.Contains(otherParent) == false)
                        continue;

                    if (node.Parents.Count <= 1)
                        break;

                    //bool clearedImmediateRelativeParent = false;
                    //foreach (Node nodeToTest in otherParent.Parents)
                    //{
                    //    if (nodeToTest == parent)
                    //    {
                    //        node.Parents.Remove(nodeToTest);
                    //        nodeToTest.Children.Remove(node);
                    //        clearedImmediateRelativeParent = true;

                    //        if (node.Parents.Count <= 1)
                    //            break;
                    //    }
                    //}

                    //if (clearedImmediateRelativeParent == false)
                    //{
                    if (parent.IsNodeAnAncestor(otherParent) == true)
                        node.RemoveParent(otherParent);

                    if (otherParent.IsNodeAnAncestor(parent) == true)
                        node.RemoveParent(parent);
                    //}
                }
            }
            List<Node> childrenToTest = new List<Node>(node.Children);

            foreach (Node childNode in childrenToTest)
                ClearRedunantLinks(childNode);
        }

        private bool IsDevelopmentInStationTechBranch(Node rootStation, Node development)
        {
            FreeAllegiance.IGCCore.Util.TreeRoot treeRoot = ((IGCCoreDevel)development.CoreObject).Root_Tree;
            //FreeAllegiance.IGCCore.Util.StationType stationType = (FreeAllegiance.IGCCore.Util.StationType)((IGCCoreStationType)rootStation.CoreObject).Type;

            FreeAllegiance.IGCCore.Util.StationType? stationType = GetStationType(rootStation, true);
            if (stationType == null)
                stationType = GetStationType(rootStation, false);

            if (stationType == null)
                return false;

            switch (treeRoot)
            {
                case FreeAllegiance.IGCCore.Util.TreeRoot.Garrison:
                    if (stationType.Value == FreeAllegiance.IGCCore.Util.StationType.Garrison)
                        return true;
                    break;

                case FreeAllegiance.IGCCore.Util.TreeRoot.Tactical:
                    if (stationType.Value == FreeAllegiance.IGCCore.Util.StationType.Tactical)
                        return true;
                    break;

                case FreeAllegiance.IGCCore.Util.TreeRoot.Expansion:
                    if (stationType.Value == FreeAllegiance.IGCCore.Util.StationType.Expansion)
                        return true;
                    break;

                case FreeAllegiance.IGCCore.Util.TreeRoot.Shipyard:
                    if (stationType.Value == FreeAllegiance.IGCCore.Util.StationType.Shipyard)
                        return true;
                    break;

                case FreeAllegiance.IGCCore.Util.TreeRoot.Supremacy:
                    if (stationType.Value == FreeAllegiance.IGCCore.Util.StationType.Supremacy)
                        return true;
                    break;
            }

            return false;
        }


        private FreeAllegiance.IGCCore.Util.StationType? GetStationType(Node stationNode, bool preferPrimaryParent)
        {
            FreeAllegiance.IGCCore.Util.StationType? stationType = null;

            List<FreeAllegiance.IGCCore.Util.StationType> validStationTypes = new List<FreeAllegiance.IGCCore.Util.StationType>(new FreeAllegiance.IGCCore.Util.StationType[]
            {
                FreeAllegiance.IGCCore.Util.StationType.Garrison,
                FreeAllegiance.IGCCore.Util.StationType.Tactical,
                FreeAllegiance.IGCCore.Util.StationType.Expansion,
                FreeAllegiance.IGCCore.Util.StationType.Shipyard,
                FreeAllegiance.IGCCore.Util.StationType.Supremacy
            });

            if (stationNode.CoreObject is IGCCoreStationType)
            {

                FreeAllegiance.IGCCore.Util.StationType stationTypeToTest = (FreeAllegiance.IGCCore.Util.StationType)((IGCCoreStationType)stationNode.CoreObject).Type;
                if (validStationTypes.Contains(stationTypeToTest) == true)
                {
                    stationType = stationTypeToTest;
                }
                else
                {
                    for (int i = 0; i < stationNode.Parents.Count; i++)
                    {
                        stationType = GetStationType(stationNode.Parents[i], preferPrimaryParent);
                        if (stationType != null)
                            break;

                        if (preferPrimaryParent == false)
                            break;
                    }
                }
            }
            else if (stationNode.CoreObject is IGCCoreDevel)
            {
                switch (((IGCCoreDevel)stationNode.CoreObject).Root_Tree)
                {
                    case FreeAllegiance.IGCCore.Util.TreeRoot.Garrison:
                        stationType = FreeAllegiance.IGCCore.Util.StationType.Garrison;
                        break;

                    case FreeAllegiance.IGCCore.Util.TreeRoot.Tactical:
                        stationType = FreeAllegiance.IGCCore.Util.StationType.Tactical;
                        break;


                    case FreeAllegiance.IGCCore.Util.TreeRoot.Expansion:
                        stationType = FreeAllegiance.IGCCore.Util.StationType.Expansion;
                        break;


                    case FreeAllegiance.IGCCore.Util.TreeRoot.Shipyard:
                        stationType = FreeAllegiance.IGCCore.Util.StationType.Shipyard;
                        break;


                    case FreeAllegiance.IGCCore.Util.TreeRoot.Supremacy:
                        stationType = FreeAllegiance.IGCCore.Util.StationType.Supremacy;
                        break;
                }
            }

            return stationType;
        }


        private bool AttachDevelopmentToRootLevelStation(IGCCoreDevel development, List<Node> rootLevelStations)
        {
            return false;
        }
        private List<Node> FindRootLevelStationNodes(List<Node> stationNodes, Core core, IGCCoreCiv targetFaction)
        {
            List<Node> rootLevelStationNodes = new List<Node>();

            foreach (Node stationNode in stationNodes)
            {
                bool stationRequiresDevelopment = false;
                foreach (IGCCoreDevel development in core.Developments)
                {
                    foreach (int prerequisiteID in stationNode.Prerequisites)
                    {
                        if (development.Techtree.HasDefinition(prerequisiteID) == true)
                        {
                            stationRequiresDevelopment = true;
                            break;
                        }
                    }

                    if (stationRequiresDevelopment == true)
                        break;
                }

                if (stationRequiresDevelopment == false)
                    rootLevelStationNodes.Add(stationNode);
            }

            return rootLevelStationNodes;
        }

        private bool DoesFactionDefinePrerequisitesForObject(Core core, IGCCoreCiv targetFaction, IGCCoreObject objectToTest)
        {

            if (objectToTest.UID == 74)
                Debugger.Break();
            Node nodeToTest = new Node(objectToTest);

            foreach (int prerequisiteID in nodeToTest.Prerequisites)
            {
                // For factions, pre is def, and def is 'no devel' defaults.
                if (/*targetFaction.Techtree.HasDefinition(prerequisiteID) == true ||*/ targetFaction.Techtree.HasPrerequisite(prerequisiteID) == true)
                    continue;


                // Is prereq defined by another development that this faction has access to?
                bool isPrereqDefinedByAvailableDevelopment = false;
                foreach (IGCCoreDevel development in core.Developments)
                {
                    if (development == objectToTest)
                        continue;

                    if (development.Techtree.HasDefinition(prerequisiteID) == true && DoesFactionDefinePrerequisitesForObject(core, targetFaction, development) == true)
                    {
                        isPrereqDefinedByAvailableDevelopment = true;
                        break;
                    }
                }

                if (isPrereqDefinedByAvailableDevelopment == true)
                    continue;





                bool isFactionDefiniedPrerequisite = false;

                foreach (IGCCoreCiv faction in core.Factions)
                {
                    // For factions, pre is def, and def is 'no devel' defaults.
                    if (/*faction.Techtree.HasDefinition(prerequisiteID) == true ||*/ faction.Techtree.HasPrerequisite(prerequisiteID) == true)
                    {
                        isFactionDefiniedPrerequisite = true;
                        break;
                    }
                }

                // The prerequisite was defined by another faction, but not defined by the target faction, 
                // so the object does not belong to the target faction. 
                if (isFactionDefiniedPrerequisite == true)
                    return false;
            }

            return true;
        }

        private void BuildNodeTreeFromCoreOld(Core core, Node rootNode)
        {
            List<Node> linearNodeList = new List<Node>();
            linearNodeList.Add(rootNode);

            foreach (IGCCoreStationType station in core.StationTypes)
            {
                //Debug.WriteLine("station: " + station.Name + ", pres: " + station.Techtree.GetPres());
                //Debug.WriteLine("station: " + station.Name + ", local: " + ParseLocalTechTreeToString(station.TechTreeLocal));

                linearNodeList.Add(new Node(station));
            }

            foreach (IGCCoreShip ship in core.Ships)
            {
                linearNodeList.Add(new Node(ship));
            }

            foreach (IGCCoreDevel devel in core.Developments)
            {
                linearNodeList.Add(new Node(devel));
            }

            SatisfyAllPrerequisites(linearNodeList, rootNode);
            Node[] temporaryNodeList = new Node[linearNodeList.Count];
            linearNodeList.CopyTo(temporaryNodeList);

            foreach (Node node in temporaryNodeList)
            {
                if (node.HasSatisfiedAllPrerequisites == false && node != rootNode)
                    linearNodeList.Remove(node);
            }

            foreach (Node currentNode in linearNodeList)
            {
                string defs = String.Empty;

                try { defs = currentNode.CoreObject.Techtree.GetDefs(); }
                catch (ArgumentOutOfRangeException) { }

                if (currentNode.CoreObject is IGCCoreStationType)
                    Debug.WriteLine("currentNode: " + currentNode.NodeName + "(" + currentNode.CoreObject.UID + ")\n\tPres: " + currentNode.CoreObject.Techtree.GetPres() + "\n\tDefs: " + defs);


            }


            foreach (Node currentNode in linearNodeList)
            {
                if (currentNode.CoreObject is IGCCoreCiv)
                    continue;

                //bool allPreReqsMet = false;

                List<Node> candidateParentNodes = new List<Node>();

                //bool containsStation = false;
                bool containsDevelopment = false;

                foreach (Node parentCandidateNode in linearNodeList)
                {
                    if (parentCandidateNode == currentNode)
                        continue;

                    if (IsParentNode(parentCandidateNode, currentNode, rootNode) == true && currentNode.IsNodeAnAncestor(parentCandidateNode) == false)
                    {
                        candidateParentNodes.Add(parentCandidateNode);

                        if (parentCandidateNode.CoreObject is IGCCoreDevel)
                            containsDevelopment = true;

                        //if (parentCandidateNode.CoreObject is IGCCoreStationType)
                        //    containsStation = true;
                    }
                }
                Node[] parentNodesToTest = new Node[candidateParentNodes.Count];
                candidateParentNodes.CopyTo(parentNodesToTest);

                foreach (Node node in parentNodesToTest)
                {
                    if (containsDevelopment == true && node.CoreObject is IGCCoreStationType)
                        candidateParentNodes.Remove(node);

                    if (containsDevelopment == true && node.CoreObject is IGCCoreShip)
                        candidateParentNodes.Remove(node);
                }

                foreach (Node parentCandidateNode in candidateParentNodes)
                {
                    if (currentNode.IsNodeAnAncestor(parentCandidateNode) == false && parentCandidateNode.IsNodeAnAncestor(currentNode) == false)
                        currentNode.SetParent(parentCandidateNode);
                }
            }

            //Node constructionTech = new Node(_rootNode, "Construction");
            //foreach (Node currentNode in linearNodeList)
            //{
            //    if (currentNode == _rootNode)
            //        continue;

            //    if (currentNode.Parents.Count == 0 && currentNode.CoreObject is IGCCoreDevel)
            //    {
            //        currentNode.SetParent(constructionTech);
            //    }
            //}

            Node generalObjects = new Node(rootNode, "Starting Ships");

            // Create the general sections for objects that don't have parents. 
            foreach (Node currentNode in linearNodeList)
            {
                if (currentNode == rootNode)
                    continue;

                if (currentNode.Parents.Count == 0)
                {
                    currentNode.SetParent(generalObjects);
                }
            }
        }



        private string ParseLocalTechTreeToString(BitArray localTechTree)
        {
            StringBuilder localIDs = new StringBuilder();

            for (int i = 0; i < localTechTree.Length; i++)
                if (localTechTree[i] == true)
                    localIDs.AppendFormat("{0} ", i);

            return localIDs.ToString().TrimEnd();
        }
        private void SatisfyAllPrerequisites(List<Node> linearNodeList, Node rootNode)
        {
            bool satisfiedOneOrMoreNodes = false;

            do
            {
                satisfiedOneOrMoreNodes = false;


                int satisfyCounter = 0;

                foreach (Node nodeToSatisfy in linearNodeList)
                {
                    if (nodeToSatisfy.HasSatisfiedAllPrerequisites == true)
                        continue;

                    //if (nodeToSatisfy == rootNode)
                    //{
                    //    foreach (int prerequisiteID in nodeToSatisfy.Prerequisites)
                    //        nodeToSatisfy.SatisfiedPrerequisites.Add(prerequisiteID);

                    //    satisfiedOneOrMoreNodes = true;
                    //    continue;
                    //}

                    foreach (int prerequisiteDefinitionID in nodeToSatisfy.Prerequisites)
                    {
                        if (nodeToSatisfy.CoreObject.UID == 239 && prerequisiteDefinitionID == 399)
                            Debug.Write("");

                        bool foundOneOrMoreNodesDefiningPrerequisiteID = false;

                        // For faction nodes, the prerequisites are the definitions, and the definitions are the defaults for 'no development'
                        if (rootNode.Prerequisites.Contains(prerequisiteDefinitionID) == true && nodeToSatisfy.SatisfiedPrerequisites.Contains(prerequisiteDefinitionID) == false)
                        {
                            foundOneOrMoreNodesDefiningPrerequisiteID = true;
                            nodeToSatisfy.SatisfiedPrerequisites.Add(prerequisiteDefinitionID);
                            satisfiedOneOrMoreNodes = true;
                            satisfyCounter++;
                            continue;
                        }


                        foreach (Node nodeToTestForDefinitions in linearNodeList)
                        {
                            if (nodeToTestForDefinitions.HasSatisfiedAllPrerequisites == false)
                                continue;

                            if (nodeToSatisfy == nodeToTestForDefinitions)
                                continue;

                            if (nodeToTestForDefinitions.Definitions.Contains(prerequisiteDefinitionID) == true && nodeToSatisfy.SatisfiedPrerequisites.Contains(prerequisiteDefinitionID) == false)
                            {
                                foundOneOrMoreNodesDefiningPrerequisiteID = true;
                                nodeToSatisfy.SatisfiedPrerequisites.Add(prerequisiteDefinitionID);
                                satisfiedOneOrMoreNodes = true;
                                satisfyCounter++;
                                break;
                            }

                            if (/*nodeToSatisfy.CoreObject is IGCCoreStationType &&*/ nodeToTestForDefinitions.CoreObject is IGCCoreStationType)
                            {
                                if (nodeToSatisfy.CoreObject.UID == 37 && prerequisiteDefinitionID == 247)
                                    Debug.Write("");

                                if (nodeToTestForDefinitions.Locals.Contains(prerequisiteDefinitionID) == true && nodeToSatisfy.SatisfiedPrerequisites.Contains(prerequisiteDefinitionID) == false)
                                {
                                    foundOneOrMoreNodesDefiningPrerequisiteID = true;
                                    nodeToSatisfy.SatisfiedPrerequisites.Add(prerequisiteDefinitionID);
                                    satisfiedOneOrMoreNodes = true;
                                    satisfyCounter++;
                                    break;
                                }
                            }

                            if (_globalDefinitions.Contains(prerequisiteDefinitionID) == true && nodeToSatisfy.SatisfiedPrerequisites.Contains(prerequisiteDefinitionID) == false)
                            {
                                foundOneOrMoreNodesDefiningPrerequisiteID = true;
                                nodeToSatisfy.SatisfiedPrerequisites.Add(prerequisiteDefinitionID);
                                satisfiedOneOrMoreNodes = true;
                                satisfyCounter++;
                                break;
                            }
                        }

                        // If no other nodes define this prerequisite, then this node is an automatically available node. 
                        if (foundOneOrMoreNodesDefiningPrerequisiteID == false)
                        {
                            //foreach (int definitionID in nodeToSatisfy.Definitions)
                            //{
                            //    if (nodeToSatisfy.Prerequisites.Contains(definitionID) == true && nodeToSatisfy.SatisfiedPrerequisites.Contains(definitionID) == false)
                            //        nodeToSatisfy.SatisfiedPrerequisites.Add(definitionID);
                            //}

                            foreach (int localDefinitionID in nodeToSatisfy.Locals)
                            {
                                if (nodeToSatisfy.Prerequisites.Contains(localDefinitionID) == true && nodeToSatisfy.SatisfiedPrerequisites.Contains(localDefinitionID) == false)
                                    nodeToSatisfy.SatisfiedPrerequisites.Add(localDefinitionID);
                            }
                        }

                    }




                    //foreach (Node nodeToTestForDefinitions in linearNodeList)
                    //{
                    //    if (nodeToSatisfy == nodeToTestForDefinitions)
                    //        continue;


                    //    foreach (int definitionID in nodeToTestForDefinitions.Definitions)
                    //    {
                    //        if (nodeToSatisfy.Prerequisites.Contains(definitionID) == true && nodeToSatisfy.SatisfiedPrerequisites.Contains(definitionID) == false)
                    //        {
                    //            if (nodeToTestForDefinitions.HasSatisfiedAllPrerequisites == true || nodeToTestForDefinitions == rootNode)
                    //            {
                    //                nodeToSatisfy.SatisfiedPrerequisites.Add(definitionID);
                    //                satisfiedOneOrMoreNodes = true;
                    //            }
                    //        }
                    //    }

                    //}
                }

                Debug.WriteLine("satisfyCounter: " + satisfyCounter);

            } while (satisfiedOneOrMoreNodes == true);
        }
        private bool IsParentNode(Node parentNodeCandidate, Node childNode, Node rootNode)
        {
            if (parentNodeCandidate.CoreObject.UID == 38 && childNode.CoreObject.UID == 3)
                Debugger.Break();

            if (parentNodeCandidate == rootNode)
            {
                if (childNode.CoreObject is IGCCoreStationType)
                    return true;
                else
                    return false;
            }

            /*
			if (parentNodeCandidate.CoreObject is IGCCoreStationType && childNode.CoreObject is IGCCoreDevel)
			{
				FreeAllegiance.IGCCore.Util.TreeRoot treeRoot = ((IGCCoreDevel)childNode.CoreObject).Root_Tree;
				FreeAllegiance.IGCCore.Util.StationType stationType = (FreeAllegiance.IGCCore.Util.StationType)((IGCCoreStationType)parentNodeCandidate.CoreObject).Type;
				switch (treeRoot)
				{
					case FreeAllegiance.IGCCore.Util.TreeRoot.Garrison:
						if (stationType == FreeAllegiance.IGCCore.Util.StationType.Garrison)
							return true;
						break;

					case FreeAllegiance.IGCCore.Util.TreeRoot.Tactical:
						if (stationType == FreeAllegiance.IGCCore.Util.StationType.Tactical)
							return true;
						break;

					case FreeAllegiance.IGCCore.Util.TreeRoot.Expansion:
						if (stationType == FreeAllegiance.IGCCore.Util.StationType.Expansion)
							return true;
						break;

					case FreeAllegiance.IGCCore.Util.TreeRoot.Shipyard:
						if (stationType == FreeAllegiance.IGCCore.Util.StationType.Shipyard)
							return true;
						break;

					case FreeAllegiance.IGCCore.Util.TreeRoot.Supremacy:
						if (stationType == FreeAllegiance.IGCCore.Util.StationType.Supremacy)
							return true;
						break;
				}
			}
			*/

            foreach (int prerequisiteID in childNode.Prerequisites)
            {
                if (parentNodeCandidate.Definitions.Contains(prerequisiteID) == true)
                {
                    if (childNode.CoreObject is IGCCoreShip /* && parentNodeCandidate.CoreObject is IGCCoreDevel */)
                    {
                        // Some ships define an ID as well as require it. For NerveGas stealth bombers, both a standard and an NG
                        // stealth have a prereq and a def for 63, which causes loops in the graph when these nodes become cross linked.
                        if (parentNodeCandidate.CoreObject is IGCCoreShip)
                        {
                            if (childNode.Definitions.Contains(prerequisiteID) == false)
                                return true;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    if (childNode.CoreObject is IGCCoreDevel && parentNodeCandidate.CoreObject is IGCCoreDevel)
                        return true;

                    if (childNode.CoreObject is IGCCoreStationType && parentNodeCandidate.CoreObject is IGCCoreDevel)
                        return true;

                    if (childNode.CoreObject is IGCCoreDevel && parentNodeCandidate.CoreObject is IGCCoreStationType)
                        return true;

                }
            }

            return false;



            //string defsString = string.Empty;

            //try
            //{
            //    defsString = parentNodeCandidate.CoreObject.Techtree.GetDefs();
            //}
            //catch (ArgumentOutOfRangeException ex)
            //{

            //}

            //foreach (string definition in defsString.Split(' '))
            //{
            //    int definitionID;
            //    if (Int32.TryParse(definition, out definitionID) == true)
            //    {
            //        if (childNode.CoreObject.Techtree.HasPrerequisite(Convert.ToInt32(definition)) == true)
            //        {
            //            //if(childNode.IsDescendentOfNode(parentNodeCandidate) == false)
            //                return true;
            //        }
            //    }
            //}

            //foreach (string prerequisite in childNode.CoreObject.Techtree.GetPres().Split(' '))
            //{
            //    if (parentNodeCandidate.CoreObject.Techtree.HasDefinition(Convert.ToInt32(prerequisite)) == true)
            //        return true;
            //}

            //return false;
        }

        public Node FindNodeContainingPoint(Point pointToFind)
        {
            return FindNodeContainingPoint(this._rootNode, pointToFind);
        }

        private Node FindNodeContainingPoint(Node nodeToTest, Point pointToFind)
        {
            if (nodeToTest.Top <= pointToFind.Y && nodeToTest.Top + nodeToTest.Height >= pointToFind.Y
                && nodeToTest.Left <= pointToFind.X && nodeToTest.Left + nodeToTest.Width >= pointToFind.X)
            {
                return nodeToTest;
            }

            foreach (Node childNode in nodeToTest.Children)
            {
                Node containingNode = FindNodeContainingPoint(childNode, pointToFind);
                if (containingNode != null)
                    return containingNode;
            }

            return null;
        }

        private delegate bool NodeStringMatchDelegate(Node nodeToTest, Regex searchExpression);

        public Node FindNodeContainingSearchString(string searchString, bool useRegex, Node lastFoundNode)
        {
            Regex searchExpression;
            //Node foundNode;
            List<Node> allMatchingNodes = new List<Node>();

            if (useRegex == true)
            {
                try
                {
                    searchExpression = new Regex(searchString, RegexOptions.IgnoreCase);
                }
                catch (Exception)
                {
                    return null;
                }

                FindAllNodesContainingSearchString(searchExpression, _rootNode, delegate (Node nodeToTest, Regex stringFinder) { return stringFinder.Match(nodeToTest.NodeName).Success; }, allMatchingNodes);
            }
            else
            {
                searchExpression = new Regex("^" + Regex.Escape(searchString) + ".*?", RegexOptions.IgnoreCase);
                FindAllNodesContainingSearchString(searchExpression, _rootNode, delegate (Node nodeToTest, Regex stringFinder) { return stringFinder.Match(nodeToTest.NodeName).Success; }, allMatchingNodes);

                searchExpression = new Regex(Regex.Escape(searchString), RegexOptions.IgnoreCase);
                FindAllNodesContainingSearchString(searchExpression, _rootNode, delegate (Node nodeToTest, Regex stringFinder) { return stringFinder.Match(nodeToTest.NodeName).Success; }, allMatchingNodes);
            }

            FindAllNodesContainingSearchString(searchExpression, _rootNode, delegate (Node nodeToTest, Regex stringFinder) { return stringFinder.Match(nodeToTest.Description).Success; }, allMatchingNodes);

            if (allMatchingNodes.Count > 0)
            {
                if (lastFoundNode == null)
                {
                    return allMatchingNodes[0];
                }
                else if (allMatchingNodes.Contains(lastFoundNode) == true)
                {
                    if (allMatchingNodes.IndexOf(lastFoundNode) >= allMatchingNodes.Count - 1)
                        return allMatchingNodes[0];
                    else
                        return allMatchingNodes[allMatchingNodes.IndexOf(lastFoundNode) + 1];
                }

            }

            return null;

            /*
			bool foundPreviousNode = false;

			if(useRegex == true)
			{
				searchExpression = new Regex(searchString, RegexOptions.IgnoreCase);
				foundNode = FindNodeContainingSearchString(searchExpression, _rootNode, delegate(Node nodeToTest, Regex stringFinder) { return stringFinder.Match(nodeToTest.NodeName).Success; }, lastFoundNode, ref foundPreviousNode);
			}
			else
			{
				
				searchExpression = new Regex("^" + Regex.Escape(searchString) + ".*?", RegexOptions.IgnoreCase);
				foundNode = FindNodeContainingSearchString(searchExpression, _rootNode, delegate(Node nodeToTest, Regex stringFinder) { return stringFinder.Match(nodeToTest.NodeName).Success; }, lastFoundNode, ref foundPreviousNode);

				if(foundNode == null)
				{
					searchExpression = new Regex(Regex.Escape(searchString), RegexOptions.IgnoreCase);
					foundNode = FindNodeContainingSearchString(searchExpression, _rootNode, delegate(Node nodeToTest, Regex stringFinder) { return stringFinder.Match(nodeToTest.NodeName).Success; }, lastFoundNode, ref foundPreviousNode);
				}
			}
			
			if(foundNode == null)
				FindNodeContainingSearchString(searchExpression, _rootNode, delegate(Node nodeToTest, Regex stringFinder) { return stringFinder.Match(nodeToTest.Description).Success; }, lastFoundNode, ref foundPreviousNode);

			return foundNode;
			 * */
        }
        private Node FindNodeContainingSearchString(Regex searchExpressiong, Node node, NodeStringMatchDelegate nodeStringMatchDelegate, Node lastFoundNode, ref bool foundPreviousNode)
        {
            if (node.ToString().IndexOf("ealth") > -1)
                Debug.Write("");
            bool stillLookingForPreviousMatchedNode = foundPreviousNode == false && lastFoundNode != null;

            if (nodeStringMatchDelegate(node, searchExpressiong) == true)
            {
                if (node == lastFoundNode)
                {
                    foundPreviousNode = true;
                    stillLookingForPreviousMatchedNode = false;
                }
                else if (stillLookingForPreviousMatchedNode == false)
                {
                    return node;
                }
            }

            foreach (Node childNode in node.Children)
            {
                Node foundNode = FindNodeContainingSearchString(searchExpressiong, childNode, nodeStringMatchDelegate, lastFoundNode, ref foundPreviousNode);
                if (foundNode != null && (foundPreviousNode == false && lastFoundNode != null) == false)
                    return foundNode;
            }

            return null;
        }

        private void FindAllNodesContainingSearchString(Regex searchExpression, Node node, NodeStringMatchDelegate nodeStringMatchDelegate, List<Node> matchingNodes)
        {
            if (nodeStringMatchDelegate(node, searchExpression) == true && matchingNodes.Contains(node) == false)
                matchingNodes.Add(node);

            foreach (Node childNode in node.Children)
                FindAllNodesContainingSearchString(searchExpression, childNode, nodeStringMatchDelegate, matchingNodes);
        }


        public Node FindNodeContainingCoreObject(IGCCoreObject coreObject)
        {
            return _rootNode.FindChildNodeContainingCoreObject(coreObject);
        }

    }
}
