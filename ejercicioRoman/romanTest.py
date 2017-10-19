import unittest

def convertToRomanString(aNumberToConvert):
	aromanString = ''
	if 	aNumberToConvert 
	for i in range(aNumberToConvert):
		aromanString += 'I'
	return aromanString

class RomanTest(unittest.TestCase):

	def testA(self):
		self.assertEquals(convertToRomanString(1),"I")

	def testB(self):
		self.assertEquals(convertToRomanString(2), "II")

	def testC(self):
		self.assertEquals(convertToRomanString(3), "III")

	def testD(self):
		self.assertEquals(convertToRomanString(4), 'IV')