namespace ContactBook;

public class DisjointSetUnion
{
    private readonly int[] _parent;
    private readonly int[] _rank;

    public DisjointSetUnion(int size)
    {
        _parent = new int[size];
        _rank   = new int[size];

        for (int i = 0; i < size; i++)
        {
            _parent[i] = i;
            _rank[i]   = 0;
        }
    }

    public int Find(int x)
    {
        if (_parent[x] != x)
        {
            _parent[x] = Find(_parent[x]); // path compression
        }

        return _parent[x];
    }

    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);

        if (rootX == rootY)
        {
            return;
        }

        if (_rank[rootX] < _rank[rootY])
        {
            _parent[rootX] = rootY;
        }
        else if (_rank[rootX] > _rank[rootY])
        {
            _parent[rootY] = rootX;
        }
        else
        {
            _parent[rootY] = rootX;
            _rank[rootX]++;
        }
    }
}