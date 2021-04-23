import math


def euclidean_distance(p, q):
    """
    Euclidean distance between points p and q in a space of any dimension.
    Input: p = [1, 2, 3], q = [1, 2, 3]
    Output: '0.00'
    """
    sum_array = [((p[i] - q[i]) ** 2) for i in range(len(p))]
    return "{:.2f}".format(math.sqrt(sum(sum_array)))
