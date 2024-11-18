using System.Collections.Generic;
using System;
using PROG_POE_PART_2.Classes;

public class BinarySearchTreeIssues
{
    public IssueNode Root { get; set; }

    public void Insert(Issue issue)
    {
        Root = Insert(Root, issue);
    }

    private IssueNode Insert(IssueNode node, Issue issue)
    {
        if (node == null)
            return new IssueNode(issue);

        if (issue.Priority < node.Data.Priority)
            node.Left = Insert(node.Left, issue);
        else if (issue.Priority > node.Data.Priority)
            node.Right = Insert(node.Right, issue);
        else
            node.Issues.Add(issue); // Add the issue to the list of issues with the same priority

        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
        return node;
    }

    private int Height(IssueNode node)
    {
        return node?.Height ?? 0;
    }

    public List<Issue> InOrderTraversal()
    {
        var issues = new List<Issue>();
        InOrderTraversal(Root, issues);
        return issues;
    }

    private void InOrderTraversal(IssueNode node, List<Issue> issues)
    {
        if (node == null)
            return;

        InOrderTraversal(node.Left, issues);
        issues.AddRange(node.Issues); // Add all issues in the node's Issues list
        InOrderTraversal(node.Right, issues);
    }
}
