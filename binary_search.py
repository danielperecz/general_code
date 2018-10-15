def binary_search(lst, q):
	if q not in lst:
		return "Element not in list."
	low = 0
	high = len(lst)
	while low < high:
		mid = (low + high) // 2
		if lst[mid] < q:
			low = mid + 1
		else:
			high = mid
	return low
