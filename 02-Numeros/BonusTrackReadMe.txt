Hicimos esta version adicional del trabajo ya que en nuestro intento mas que engorroso de sacar la mayor cantidad de ifs posibles, es casi seguro que hayamos roto 
el patron de diseño orientado a objetos. Pero bueno, al menos pasa todos los tests.

Modificamos el test donde se instancia Entero(0) por new Cero()
y Entero(1) por new Unidad(), ya que no permitimos estas instancias de entero

No es lo mas elegante, pero para no cambiar todas las apariciones de Entero por EnterosDistintosACeroYUno, dejamos el nombre igual
Aunque no permitimos en todo el codigo que se creen instancias de Entero(0) o Entero(1) ya que esto seria un acomplamiento 
entre las clases Entero y Cero o Unidad. Agregamos unos asserts que se fijan que en toda creacion de fraccion, el resultado 
final del numerador no puede ser de tipo Entero y equivalente a 1 u 0 en simultaneo.

Por esto, cambiamos las properties de Fraccion de tal manera que Numerador ahora es un Numero, pero denominador sigue siendo entero
De esta manera la fraccion 1/5 se identifica como Numerador = una instancia de Unidad, y denominador siempre un Entero, ya que
este siempre sera distinto de 0 o 1, porque en el primer caso tiene que saltar una excepcion, y en el segundo, la fraccion seria retornada como un Entero o Unidad

Apreciariamos mucho un buen ejemplo de como se podria hacer esto mejor

 