using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FreeAllegiance.IGCCore.Modules;
using System.Diagnostics;
using System.Collections;
using System.Text.Json.Serialization;

namespace CoreExporter
{
    public class Node
    {
        public readonly string NodeName;
        public readonly string Description = string.Empty;

        [JsonIgnore]
        public List<Node> Parents = new List<Node>();
        public List<Node> Children = new List<Node>();

        //public bool HasBeenRendered = false;

        private int _row = 0;
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public int Column = 0;

        // Rendered Dimensions
        public int Top = 0;
        public int Left = 0;
        public int Width = 0;
        public int Height = 0;

        public readonly IGCCoreObject CoreObject;
        public List<int> Definitions = new List<int>();
        public List<int> Prerequisites = new List<int>();
        public List<int> Locals = new List<int>();

        public List<int> SatisfiedPrerequisites = new List<int>();

        public bool HasSatisfiedAllPrerequisites
        {
            get { return SatisfiedPrerequisites.Count == Prerequisites.Count; }
        }

        public Node(IGCCoreObject coreObject)
        {
            this.CoreObject = coreObject;
            this.NodeName = coreObject.Name + " (" + coreObject.UID + ")";

            try
            {
                Definitions = ParseIntCollectionString(coreObject.Techtree.GetDefs());
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                Prerequisites = ParseIntCollectionString(coreObject.Techtree.GetPres());
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            if (coreObject is IGCCoreStationType)
            {
                Locals = ParseTechTreeFromBitArray(((IGCCoreStationType)coreObject).TechTreeLocal);
                Description = ((IGCCoreStationType)coreObject).Description;
            }
            else if (coreObject is IGCCoreDevel)
            {
                Description = ((IGCCoreDevel)coreObject).Description;
            }
            else if (coreObject is IGCCoreShip)
            {
                Description = ((IGCCoreShip)coreObject).Description;
            }
        }

        public Node(Node parentNode, string nodeName)
        {
            this.NodeName = nodeName;

            if (parentNode != null)
            {
                this.Parents.Add(parentNode);
                parentNode.Children.Add(this);
            }
        }

        public void SetParent(Node parentNode)
        {
            if (this.CoreObject.UID == 301 && parentNode.CoreObject.UID == 47)
                Debug.Write("");

            if (this.IsNodeAnAncestor(parentNode) == true)
                throw new Exception("This parent node is alread a parent of this child, or one of this child's ancestors.");

            if (parentNode.IsNodeAnAncestor(this) == true)
                throw new Exception("Attempting to set a child node of a parent node as the parent node's parent is illegial.");

            if (this == parentNode)
                throw new Exception("Cannot assign a node as it's own parent.");

            this.Parents.Add(parentNode);
            parentNode.Children.Add(this);
        }

        public void RemoveParent(Node parentNode)
        {
            this.Parents.Remove(parentNode);
            parentNode.Children.Remove(this);
        }

        public int Depth
        {
            get
            {
                int depth = 0;
                for (Node currentNode = this; currentNode.Parents.Count > 0; currentNode = currentNode.Parents[0])
                    depth++;

                return depth;
            }
        }

        public bool IsNodeAnAncestor(Node ancestorNodeCandidate)
        {
            if (this.CoreObject.UID == 46)
                Debug.Write("");

            //Debug.WriteLine(this.NodeName);
            foreach (Node parentNode in this.Parents)
            {
                if (parentNode == ancestorNodeCandidate)
                    return true;

                if (parentNode.IsNodeAnAncestor(ancestorNodeCandidate) == true)
                    return true;
            }

            return false;
        }

        public bool IsNodeADecendant(Node descendentNodeCandidate)
        {
            foreach (Node childNode in this.Children)
            {
                if (childNode == descendentNodeCandidate)
                    return true;

                if (childNode.IsNodeADecendant(descendentNodeCandidate) == true)
                    return true;
            }

            return false;
        }


        public bool AreNodesRelated(Node nodeToTest)
        {
            //if (nodeToTest.CoreObject.UID == 211)
            //	Debugger.Break();

            //Debug.WriteLine(nodeToTest.NodeName);

            if (this.IsNodeAnAncestor(nodeToTest) == true)
                return true;

            if (nodeToTest.IsNodeAnAncestor(this) == true)
                return true;

            if (this.IsNodeADecendant(nodeToTest) == true)
                return true;

            if (nodeToTest.IsNodeADecendant(this) == true)
                return true;

            return false;
        }

        private List<int> ParseIntCollectionString(string intCollection)
        {
            List<int> returnValue = new List<int>();

            foreach (string intString in intCollection.Split(' '))
            {
                int value;
                if (Int32.TryParse(intString, out value) == true)
                    returnValue.Add(value);
            }

            return returnValue;
        }

        private List<int> ParseTechTreeFromBitArray(BitArray localTechTree)
        {
            List<int> localIDs = new List<int>();

            for (int i = 0; i < localTechTree.Length; i++)
                if (localTechTree[i] == true)
                    localIDs.Add(i);

            return localIDs;
        }

        public bool Defines(Node nodeToTest, bool testLocals)
        {
            foreach (int preqrequisiteID in nodeToTest.Prerequisites)
            {
                if (this.Definitions.Contains(preqrequisiteID) == true)
                    return true;

                if (testLocals == true && this.Locals.Contains(preqrequisiteID) == true)
                    return true;
            }

            return false;
        }

        public void InsertParent(Node parentNode, int index)
        {
            this.Parents.Insert(0, parentNode);
            parentNode.Children.Add(this);
        }

        public override string ToString()
        {
            return this.NodeName;
        }

        public Node FindChildNodeContainingCoreObject(IGCCoreObject coreObject)
        {
            if (this.CoreObject == coreObject)
                return this;

            foreach (Node childNode in this.Children)
            {
                Node foundNode = childNode.FindChildNodeContainingCoreObject(coreObject);
                if (foundNode != null)
                    return foundNode;
            }

            return null;
        }
    }
}
