def lowest_non_zero_in_list(a):
    a = sorted(a)
    for x in a:
        if x > 1:
            return x
    return 1


def lowest_not_in_list(a):
    a = set(a)
    i = 1
    while True:
        if i not in a:
            return i
        i += 1
    return 0

def solution(a):
    lowest = lowest_non_zero_in_list(a)
    if (len(a) == 1) or (lowest > 1):
        return lowest_not_in_list(a) 
    else:
        return 1
