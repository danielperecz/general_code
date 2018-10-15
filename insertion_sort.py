def insertion_sort(lst):
	i = 1
	while i < len(lst):
		v = lst[i]
		p = i
		while 0 < p and v < lst[p-1]:
			lst[p] = lst[p-1]
			p -= 1
		lst[p] = v
		i += 1
	return lst
