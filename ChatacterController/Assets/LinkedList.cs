using System;
using UnityEngine;

public class LinkedList
{
    Node head;
	public LinkedList()
	{

	}
    public void Add(string data)
    {
        if (this.head == null)
        {
            this.head = new Node(data);
            return;
        }
        Node node = head;
        while (node.next !=null)
        {
            node = node.next;
        }
        
            node.next = new Node(data);     
       
    }
    public void TraverseAndPring()
    {
        Node node = head;
        while (node != null)
        {
            Debug.Log(node.data);
            node = node.next;
        }

    }
}
