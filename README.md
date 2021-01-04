# D3Sharp
C# implementation for [D3.js](https://github.com/d3/d3)  on .Net Standard 2.1 .

## Available

* [D3Sharp.QuadTree](#d3sharpquadtree) for [d3-quadtree](https://github.com/d3/d3-quadtree).
* [D3Sharp.Force](#d3sharpforce) for [d3-force](https://github.com/d3/d3-force).



## Examples

### D3Sharp.QuadTree

Definition:

```c#
public class QuadTree<TData, TNode>
    where TData : IQuadData
    where TNode : QuadNode<TData>, new()
```



By default use `QuadNode` create tree nodes:

``` c#
...
using D3Sharp.QuadTree;

public class CustomData : IQuadData
{
     public double X { get; set; } = double.NaN;
     public double Y { get; set; } = double.NaN;

     public string Location => X + "," + Y;
     public int Id { get; set; }
}
    
var q = new QuadTree<CustomData, QuadNode<CustomData>>();
q.Extent(0, 0, 2, 2).Add(new CustomData{X=3,Y=1,CustomField="Custom"});
Console.WriteLine($"Bounds = {q.Extents}");
//output: new double[,] { { 0, 0 }, { 4, 4 } }
Console.WriteLine($"Node type = {q.Root.GetType().Name}");
//output: QuadNode`1
```

Use custom tree node:

 ``` c#
...

public class CustomNode<T> : QuadNode<T> where T : CustomData
{
    public int Index { get; set; }

    public override T Data
    {
        get => base.Data;
        set
        {
            base.Data = value;
            this.Index = value.Id;
        }
     }
}

var datas = new List<CustomData> {
    new CustomData{X=0,Y=0,Id=0},
    new CustomData{X=0.9,Y=0.9,Id=1},
};
    
var q = new QuadTree<CustomData, CustomNode<CustomData>>(datas);
Console.WriteLine($"Bounds = {q.Extents}");
//output: new double[,] { { 0, 0 }, { 1, 1 } }
Console.WriteLine($"Node type = {q.Root.GetType().Name}");
//output: CustomNode`1
 ```

### D3Sharp.Force

Definition:

``` csharp
public class Link
public interface INode : QuadTree.IQuadData 
public abstract class Force<TNode> : IDisposable where TNode : INode
public class Simulation<TNode> : IDisposable where TNode : INode      
```

Sample:

```csharp
...
using D3Sharp.Force;
...

var simulation = new Simulation<Node>(Nodes)
    .AddForce("Links", new ForceLink<Node,Link>(Links)
    .AddForce("Centering", new ForceCenter<Node>(300, 300).SetStrength(1))
    .AddForce("Collision", new ForceCollide<Node>(50, 0.3, 5)
    .AddForce("Many-Body", new ForceManyBody<Node>().SetStrength(-100);
simulation.Start();
```

Screenshot:

![force-demo](https://github.com/leisn/D3Sharp/blob/master/Screenshot/force-demo.png)
