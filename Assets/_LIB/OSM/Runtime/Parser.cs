using System.Collections.Generic;
using FunkySheep.JSON;

namespace FunkySheep.OSM
{
  public class Data
  {
    public List<Node> nodes = new List<Node>();
    public List<Way> ways = new List<Way>();
    public List<Relation> relations = new List<Relation>();

    public void AddElement(JSONNode elementJSON)
    {
      switch ((string)elementJSON["type"])
        {
            case "way":
                AddWay(elementJSON);
                break;
            case "relation":
                AddRelation(elementJSON);
                break;
            default:
                break;
        }
    }

    public Way AddWay(JSONNode wayJSON)
    {
      Way way = ways.Find(way => way.id == wayJSON["id"]);

      if (way == null)
      {
        way = new Way(wayJSON["id"]);
      }
      // Add the node id to the nodes list
      JSONArray nodes = wayJSON["nodes"].AsArray;
      JSONArray geometries = wayJSON["geometry"].AsArray;

      for (int i = 0; i < geometries.Count; i++)
      {
        if (way.nodes.Find(node => node.id == nodes[i]) == null)
        {
          Node node = AddNode(nodes[i].AsLong, geometries[i]["lat"].AsDouble, geometries[i]["lon"].AsDouble);
          way.nodes.Add(node);
        }
      }

      JSONObject tags = wayJSON["tags"].AsObject;

      foreach (KeyValuePair<string, JSONNode> tag in (JSONObject)tags)
      {
          way.tags.Add(new Tag(tag.Key, tag.Value));
      }
      
      ways.Add(way);
      return way;
    }

    public Relation AddRelation(JSONNode relationJSON)
    {
      Relation relation = relations.Find(relation => relation.id == relationJSON["id"]);

      if (relation == null)
      {
        relation = new Relation(relationJSON["id"]);
      }

      JSONArray members = relationJSON["members"].AsArray;

      for (int i = 0; i < members.Count; i++)
      {
        members[i].Add("id", members[i]["ref"]);
        switch ((string)members[i]["type"])
        {
            case "way":
                relation.ways.Add(AddWay(members[i]));
                break;
            default:
                break;
        }
      }

      JSONObject tags = relationJSON["tags"].AsObject;

      foreach (KeyValuePair<string, JSONNode> tag in (JSONObject)tags)
      {
        relation.tags.Add(new Tag(tag.Key, tag.Value));
      }

      relations.Add(relation);
      return relation;
    }

    public Node AddNode(long id, double latitude, double longitude)
    {
      // Find by ID
      Node node = nodes.Find(node => node.id == id);
      if (node == null)
      {
        // Find by lat/lon
        node = nodes.Find(node => node.latitude == latitude && node.longitude == longitude);
      }
      
      if (node == null) {
        node = new Node(id, latitude, longitude);
      }

      nodes.Add(node);
      return node;
    }
  }

  public static class Parser 
  {
    public static Data Parse(JSONNode jsonData)
    {
      JSONArray elements = jsonData["elements"].AsArray;
      Data data = new Data();

      for (int i = 0; i < elements.Count; i++)
      {
        data.AddElement(elements[i]);
      }

      return data;
    }

    public static Data Parse(string textData)
    {
      JSONNode jsonData = JSONNode.Parse(textData);
      return Parse(jsonData);
    }

    public static Data Parse(byte[] rawData)
    {
      string textData = System.Text.Encoding.UTF8.GetString(rawData);
      return Parse(textData);
    }
  }  
}
