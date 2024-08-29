//*****************************************************************************
//** 947. Most Stones Removed with Same Row or Column  leetcode              **
//*****************************************************************************

typedef struct {
    int* p;
    int* size;
    int n;
} UnionFind;

UnionFind* UnionFind_create(int n) {
    UnionFind* uf = (UnionFind*)malloc(sizeof(UnionFind));
    uf->p = (int*)malloc(n * sizeof(int));
    uf->size = (int*)malloc(n * sizeof(int));
    uf->n = n;
    for (int i = 0; i < n; i++) {
        uf->p[i] = i;
        uf->size[i] = 1;
    }
    return uf;
}

int UnionFind_find(UnionFind* uf, int x) {
    if (uf->p[x] != x) {
        uf->p[x] = UnionFind_find(uf, uf->p[x]);
    }
    return uf->p[x];
}

int UnionFind_unite(UnionFind* uf, int a, int b) {
    int pa = UnionFind_find(uf, a);
    int pb = UnionFind_find(uf, b);
    if (pa == pb) {
        return 0;
    }
    if (uf->size[pa] > uf->size[pb]) {
        uf->p[pb] = pa;
        uf->size[pa] += uf->size[pb];
    } else {
        uf->p[pa] = pb;
        uf->size[pb] += uf->size[pa];
    }
    return 1;
}

void UnionFind_free(UnionFind* uf) {
    free(uf->p);
    free(uf->size);
    free(uf);
}

int removeStones(int** stones, int stonesSize, int* stonesColSize) {
    int m = 10001;
    UnionFind* uf = UnionFind_create(m << 1);
    for (int i = 0; i < stonesSize; i++) {
        UnionFind_unite(uf, stones[i][0], stones[i][1] + m);
    }
    
    int* uniqueParents = (int*)calloc(stonesSize, sizeof(int));
    int uniqueCount = 0;
    for (int i = 0; i < stonesSize; i++) {
        int root = UnionFind_find(uf, stones[i][0]);
        int isUnique = 1;
        for (int j = 0; j < uniqueCount; j++) {
            if (uniqueParents[j] == root) {
                isUnique = 0;
                break;
            }
        }
        if (isUnique) {
            uniqueParents[uniqueCount++] = root;
        }
    }

    int result = stonesSize - uniqueCount;
    free(uniqueParents);
    UnionFind_free(uf);
    return result;
}