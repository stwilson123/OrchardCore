/*!
 * module-loader.js v0.0.13
 * (c) 2014-2020 stwilson
 * Released under the MIT License.
 */
import Vue from 'vue';
import { Prop, Component as Component$1 } from 'vue-property-decorator';
export { Prop } from 'vue-property-decorator';

var commonjsGlobal = typeof globalThis !== 'undefined' ? globalThis : typeof window !== 'undefined' ? window : typeof global !== 'undefined' ? global : typeof self !== 'undefined' ? self : {};

function unwrapExports (x) {
	return x && x.__esModule && Object.prototype.hasOwnProperty.call(x, 'default') ? x['default'] : x;
}

function createCommonjsModule(fn, module) {
	return module = { exports: {} }, fn(module, module.exports), module.exports;
}

var check = function (it) {
  return it && it.Math == Math && it;
}; // https://github.com/zloirock/core-js/issues/86#issuecomment-115759028


var global_1 = // eslint-disable-next-line no-undef
check(typeof globalThis == 'object' && globalThis) || check(typeof window == 'object' && window) || check(typeof self == 'object' && self) || check(typeof commonjsGlobal == 'object' && commonjsGlobal) || // eslint-disable-next-line no-new-func
Function('return this')();

var fails = function (exec) {
  try {
    return !!exec();
  } catch (error) {
    return true;
  }
};

var descriptors = !fails(function () {
  return Object.defineProperty({}, 'a', {
    get: function () {
      return 7;
    }
  }).a != 7;
});

var nativePropertyIsEnumerable = {}.propertyIsEnumerable;
var getOwnPropertyDescriptor = Object.getOwnPropertyDescriptor; // Nashorn ~ JDK8 bug

var NASHORN_BUG = getOwnPropertyDescriptor && !nativePropertyIsEnumerable.call({
  1: 2
}, 1); // `Object.prototype.propertyIsEnumerable` method implementation
// https://tc39.github.io/ecma262/#sec-object.prototype.propertyisenumerable

var f = NASHORN_BUG ? function propertyIsEnumerable(V) {
  var descriptor = getOwnPropertyDescriptor(this, V);
  return !!descriptor && descriptor.enumerable;
} : nativePropertyIsEnumerable;
var objectPropertyIsEnumerable = {
  f: f
};

var createPropertyDescriptor = function (bitmap, value) {
  return {
    enumerable: !(bitmap & 1),
    configurable: !(bitmap & 2),
    writable: !(bitmap & 4),
    value: value
  };
};

var toString = {}.toString;

var classofRaw = function (it) {
  return toString.call(it).slice(8, -1);
};

var split = ''.split; // fallback for non-array-like ES3 and non-enumerable old V8 strings

var indexedObject = fails(function () {
  // throws an error in rhino, see https://github.com/mozilla/rhino/issues/346
  // eslint-disable-next-line no-prototype-builtins
  return !Object('z').propertyIsEnumerable(0);
}) ? function (it) {
  return classofRaw(it) == 'String' ? split.call(it, '') : Object(it);
} : Object;

// `RequireObjectCoercible` abstract operation
// https://tc39.github.io/ecma262/#sec-requireobjectcoercible
var requireObjectCoercible = function (it) {
  if (it == undefined) throw TypeError("Can't call method on " + it);
  return it;
};

var toIndexedObject = function (it) {
  return indexedObject(requireObjectCoercible(it));
};

var isObject = function (it) {
  return typeof it === 'object' ? it !== null : typeof it === 'function';
};

// https://tc39.github.io/ecma262/#sec-toprimitive
// instead of the ES6 spec version, we didn't implement @@toPrimitive case
// and the second argument - flag - preferred type is a string

var toPrimitive = function (input, PREFERRED_STRING) {
  if (!isObject(input)) return input;
  var fn, val;
  if (PREFERRED_STRING && typeof (fn = input.toString) == 'function' && !isObject(val = fn.call(input))) return val;
  if (typeof (fn = input.valueOf) == 'function' && !isObject(val = fn.call(input))) return val;
  if (!PREFERRED_STRING && typeof (fn = input.toString) == 'function' && !isObject(val = fn.call(input))) return val;
  throw TypeError("Can't convert object to primitive value");
};

var hasOwnProperty = {}.hasOwnProperty;

var has = function (it, key) {
  return hasOwnProperty.call(it, key);
};

var document$1 = global_1.document; // typeof document.createElement is 'object' in old IE

var EXISTS = isObject(document$1) && isObject(document$1.createElement);

var documentCreateElement = function (it) {
  return EXISTS ? document$1.createElement(it) : {};
};

var ie8DomDefine = !descriptors && !fails(function () {
  return Object.defineProperty(documentCreateElement('div'), 'a', {
    get: function () {
      return 7;
    }
  }).a != 7;
});

var nativeGetOwnPropertyDescriptor = Object.getOwnPropertyDescriptor; // `Object.getOwnPropertyDescriptor` method
// https://tc39.github.io/ecma262/#sec-object.getownpropertydescriptor

var f$1 = descriptors ? nativeGetOwnPropertyDescriptor : function getOwnPropertyDescriptor(O, P) {
  O = toIndexedObject(O);
  P = toPrimitive(P, true);
  if (ie8DomDefine) try {
    return nativeGetOwnPropertyDescriptor(O, P);
  } catch (error) {
    /* empty */
  }
  if (has(O, P)) return createPropertyDescriptor(!objectPropertyIsEnumerable.f.call(O, P), O[P]);
};
var objectGetOwnPropertyDescriptor = {
  f: f$1
};

var replacement = /#|\.prototype\./;

var isForced = function (feature, detection) {
  var value = data[normalize(feature)];
  return value == POLYFILL ? true : value == NATIVE ? false : typeof detection == 'function' ? fails(detection) : !!detection;
};

var normalize = isForced.normalize = function (string) {
  return String(string).replace(replacement, '.').toLowerCase();
};

var data = isForced.data = {};
var NATIVE = isForced.NATIVE = 'N';
var POLYFILL = isForced.POLYFILL = 'P';
var isForced_1 = isForced;

var path = {};

var aFunction = function (it) {
  if (typeof it != 'function') {
    throw TypeError(String(it) + ' is not a function');
  }

  return it;
};

var bindContext = function (fn, that, length) {
  aFunction(fn);
  if (that === undefined) return fn;

  switch (length) {
    case 0:
      return function () {
        return fn.call(that);
      };

    case 1:
      return function (a) {
        return fn.call(that, a);
      };

    case 2:
      return function (a, b) {
        return fn.call(that, a, b);
      };

    case 3:
      return function (a, b, c) {
        return fn.call(that, a, b, c);
      };
  }

  return function ()
  /* ...args */
  {
    return fn.apply(that, arguments);
  };
};

var anObject = function (it) {
  if (!isObject(it)) {
    throw TypeError(String(it) + ' is not an object');
  }

  return it;
};

var nativeDefineProperty = Object.defineProperty; // `Object.defineProperty` method
// https://tc39.github.io/ecma262/#sec-object.defineproperty

var f$2 = descriptors ? nativeDefineProperty : function defineProperty(O, P, Attributes) {
  anObject(O);
  P = toPrimitive(P, true);
  anObject(Attributes);
  if (ie8DomDefine) try {
    return nativeDefineProperty(O, P, Attributes);
  } catch (error) {
    /* empty */
  }
  if ('get' in Attributes || 'set' in Attributes) throw TypeError('Accessors not supported');
  if ('value' in Attributes) O[P] = Attributes.value;
  return O;
};
var objectDefineProperty = {
  f: f$2
};

var createNonEnumerableProperty = descriptors ? function (object, key, value) {
  return objectDefineProperty.f(object, key, createPropertyDescriptor(1, value));
} : function (object, key, value) {
  object[key] = value;
  return object;
};

var getOwnPropertyDescriptor$1 = objectGetOwnPropertyDescriptor.f;

var wrapConstructor = function (NativeConstructor) {
  var Wrapper = function (a, b, c) {
    if (this instanceof NativeConstructor) {
      switch (arguments.length) {
        case 0:
          return new NativeConstructor();

        case 1:
          return new NativeConstructor(a);

        case 2:
          return new NativeConstructor(a, b);
      }

      return new NativeConstructor(a, b, c);
    }

    return NativeConstructor.apply(this, arguments);
  };

  Wrapper.prototype = NativeConstructor.prototype;
  return Wrapper;
};
/*
  options.target      - name of the target object
  options.global      - target is the global object
  options.stat        - export as static methods of target
  options.proto       - export as prototype methods of target
  options.real        - real prototype method for the `pure` version
  options.forced      - export even if the native feature is available
  options.bind        - bind methods to the target, required for the `pure` version
  options.wrap        - wrap constructors to preventing global pollution, required for the `pure` version
  options.unsafe      - use the simple assignment of property instead of delete + defineProperty
  options.sham        - add a flag to not completely full polyfills
  options.enumerable  - export as enumerable property
  options.noTargetGet - prevent calling a getter on target
*/


var _export = function (options, source) {
  var TARGET = options.target;
  var GLOBAL = options.global;
  var STATIC = options.stat;
  var PROTO = options.proto;
  var nativeSource = GLOBAL ? global_1 : STATIC ? global_1[TARGET] : (global_1[TARGET] || {}).prototype;
  var target = GLOBAL ? path : path[TARGET] || (path[TARGET] = {});
  var targetPrototype = target.prototype;
  var FORCED, USE_NATIVE, VIRTUAL_PROTOTYPE;
  var key, sourceProperty, targetProperty, nativeProperty, resultProperty, descriptor;

  for (key in source) {
    FORCED = isForced_1(GLOBAL ? key : TARGET + (STATIC ? '.' : '#') + key, options.forced); // contains in native

    USE_NATIVE = !FORCED && nativeSource && has(nativeSource, key);
    targetProperty = target[key];
    if (USE_NATIVE) if (options.noTargetGet) {
      descriptor = getOwnPropertyDescriptor$1(nativeSource, key);
      nativeProperty = descriptor && descriptor.value;
    } else nativeProperty = nativeSource[key]; // export native or implementation

    sourceProperty = USE_NATIVE && nativeProperty ? nativeProperty : source[key];
    if (USE_NATIVE && typeof targetProperty === typeof sourceProperty) continue; // bind timers to global for call from export context

    if (options.bind && USE_NATIVE) resultProperty = bindContext(sourceProperty, global_1); // wrap global constructors for prevent changs in this version
    else if (options.wrap && USE_NATIVE) resultProperty = wrapConstructor(sourceProperty); // make static versions for prototype methods
      else if (PROTO && typeof sourceProperty == 'function') resultProperty = bindContext(Function.call, sourceProperty); // default case
        else resultProperty = sourceProperty; // add a flag to not completely full polyfills

    if (options.sham || sourceProperty && sourceProperty.sham || targetProperty && targetProperty.sham) {
      createNonEnumerableProperty(resultProperty, 'sham', true);
    }

    target[key] = resultProperty;

    if (PROTO) {
      VIRTUAL_PROTOTYPE = TARGET + 'Prototype';

      if (!has(path, VIRTUAL_PROTOTYPE)) {
        createNonEnumerableProperty(path, VIRTUAL_PROTOTYPE, {});
      } // export virtual prototype methods


      path[VIRTUAL_PROTOTYPE][key] = sourceProperty; // export real prototype methods

      if (options.real && targetPrototype && !targetPrototype[key]) {
        createNonEnumerableProperty(targetPrototype, key, sourceProperty);
      }
    }
  }
};

var aFunction$1 = function (variable) {
  return typeof variable == 'function' ? variable : undefined;
};

var getBuiltIn = function (namespace, method) {
  return arguments.length < 2 ? aFunction$1(path[namespace]) || aFunction$1(global_1[namespace]) : path[namespace] && path[namespace][method] || global_1[namespace] && global_1[namespace][method];
};

var isPure = true;

var nativeSymbol = !!Object.getOwnPropertySymbols && !fails(function () {
  // Chrome 38 Symbol has incorrect toString conversion
  // eslint-disable-next-line no-undef
  return !String(Symbol());
});

var useSymbolAsUid = nativeSymbol // eslint-disable-next-line no-undef
&& !Symbol.sham // eslint-disable-next-line no-undef
&& typeof Symbol.iterator == 'symbol';

// https://tc39.github.io/ecma262/#sec-isarray

var isArray = Array.isArray || function isArray(arg) {
  return classofRaw(arg) == 'Array';
};

// https://tc39.github.io/ecma262/#sec-toobject

var toObject = function (argument) {
  return Object(requireObjectCoercible(argument));
};

var ceil = Math.ceil;
var floor = Math.floor; // `ToInteger` abstract operation
// https://tc39.github.io/ecma262/#sec-tointeger

var toInteger = function (argument) {
  return isNaN(argument = +argument) ? 0 : (argument > 0 ? floor : ceil)(argument);
};

var min = Math.min; // `ToLength` abstract operation
// https://tc39.github.io/ecma262/#sec-tolength

var toLength = function (argument) {
  return argument > 0 ? min(toInteger(argument), 0x1FFFFFFFFFFFFF) : 0; // 2 ** 53 - 1 == 9007199254740991
};

var max = Math.max;
var min$1 = Math.min; // Helper for a popular repeating case of the spec:
// Let integer be ? ToInteger(index).
// If integer < 0, let result be max((length + integer), 0); else let result be min(integer, length).

var toAbsoluteIndex = function (index, length) {
  var integer = toInteger(index);
  return integer < 0 ? max(integer + length, 0) : min$1(integer, length);
};

var createMethod = function (IS_INCLUDES) {
  return function ($this, el, fromIndex) {
    var O = toIndexedObject($this);
    var length = toLength(O.length);
    var index = toAbsoluteIndex(fromIndex, length);
    var value; // Array#includes uses SameValueZero equality algorithm
    // eslint-disable-next-line no-self-compare

    if (IS_INCLUDES && el != el) while (length > index) {
      value = O[index++]; // eslint-disable-next-line no-self-compare

      if (value != value) return true; // Array#indexOf ignores holes, Array#includes - not
    } else for (; length > index; index++) {
      if ((IS_INCLUDES || index in O) && O[index] === el) return IS_INCLUDES || index || 0;
    }
    return !IS_INCLUDES && -1;
  };
};

var arrayIncludes = {
  // `Array.prototype.includes` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.includes
  includes: createMethod(true),
  // `Array.prototype.indexOf` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.indexof
  indexOf: createMethod(false)
};

var hiddenKeys = {};

var indexOf = arrayIncludes.indexOf;

var objectKeysInternal = function (object, names) {
  var O = toIndexedObject(object);
  var i = 0;
  var result = [];
  var key;

  for (key in O) !has(hiddenKeys, key) && has(O, key) && result.push(key); // Don't enum bug & hidden keys


  while (names.length > i) if (has(O, key = names[i++])) {
    ~indexOf(result, key) || result.push(key);
  }

  return result;
};

// IE8- don't enum bug keys
var enumBugKeys = ['constructor', 'hasOwnProperty', 'isPrototypeOf', 'propertyIsEnumerable', 'toLocaleString', 'toString', 'valueOf'];

// https://tc39.github.io/ecma262/#sec-object.keys

var objectKeys = Object.keys || function keys(O) {
  return objectKeysInternal(O, enumBugKeys);
};

// https://tc39.github.io/ecma262/#sec-object.defineproperties

var objectDefineProperties = descriptors ? Object.defineProperties : function defineProperties(O, Properties) {
  anObject(O);
  var keys = objectKeys(Properties);
  var length = keys.length;
  var index = 0;
  var key;

  while (length > index) objectDefineProperty.f(O, key = keys[index++], Properties[key]);

  return O;
};

var html = getBuiltIn('document', 'documentElement');

var setGlobal = function (key, value) {
  try {
    createNonEnumerableProperty(global_1, key, value);
  } catch (error) {
    global_1[key] = value;
  }

  return value;
};

var SHARED = '__core-js_shared__';
var store = global_1[SHARED] || setGlobal(SHARED, {});
var sharedStore = store;

var shared = createCommonjsModule(function (module) {
  (module.exports = function (key, value) {
    return sharedStore[key] || (sharedStore[key] = value !== undefined ? value : {});
  })('versions', []).push({
    version: '3.6.1',
    mode:  'pure' ,
    copyright: '© 2019 Denis Pushkarev (zloirock.ru)'
  });
});

var id = 0;
var postfix = Math.random();

var uid = function (key) {
  return 'Symbol(' + String(key === undefined ? '' : key) + ')_' + (++id + postfix).toString(36);
};

var keys = shared('keys');

var sharedKey = function (key) {
  return keys[key] || (keys[key] = uid(key));
};

var GT = '>';
var LT = '<';
var PROTOTYPE = 'prototype';
var SCRIPT = 'script';
var IE_PROTO = sharedKey('IE_PROTO');

var EmptyConstructor = function () {
  /* empty */
};

var scriptTag = function (content) {
  return LT + SCRIPT + GT + content + LT + '/' + SCRIPT + GT;
}; // Create object with fake `null` prototype: use ActiveX Object with cleared prototype


var NullProtoObjectViaActiveX = function (activeXDocument) {
  activeXDocument.write(scriptTag(''));
  activeXDocument.close();
  var temp = activeXDocument.parentWindow.Object;
  activeXDocument = null; // avoid memory leak

  return temp;
}; // Create object with fake `null` prototype: use iframe Object with cleared prototype


var NullProtoObjectViaIFrame = function () {
  // Thrash, waste and sodomy: IE GC bug
  var iframe = documentCreateElement('iframe');
  var JS = 'java' + SCRIPT + ':';
  var iframeDocument;
  iframe.style.display = 'none';
  html.appendChild(iframe); // https://github.com/zloirock/core-js/issues/475

  iframe.src = String(JS);
  iframeDocument = iframe.contentWindow.document;
  iframeDocument.open();
  iframeDocument.write(scriptTag('document.F=Object'));
  iframeDocument.close();
  return iframeDocument.F;
}; // Check for document.domain and active x support
// No need to use active x approach when document.domain is not set
// see https://github.com/es-shims/es5-shim/issues/150
// variation of https://github.com/kitcambridge/es5-shim/commit/4f738ac066346
// avoid IE GC bug


var activeXDocument;

var NullProtoObject = function () {
  try {
    /* global ActiveXObject */
    activeXDocument = document.domain && new ActiveXObject('htmlfile');
  } catch (error) {
    /* ignore */
  }

  NullProtoObject = activeXDocument ? NullProtoObjectViaActiveX(activeXDocument) : NullProtoObjectViaIFrame();
  var length = enumBugKeys.length;

  while (length--) delete NullProtoObject[PROTOTYPE][enumBugKeys[length]];

  return NullProtoObject();
};

hiddenKeys[IE_PROTO] = true; // `Object.create` method
// https://tc39.github.io/ecma262/#sec-object.create

var objectCreate = Object.create || function create(O, Properties) {
  var result;

  if (O !== null) {
    EmptyConstructor[PROTOTYPE] = anObject(O);
    result = new EmptyConstructor();
    EmptyConstructor[PROTOTYPE] = null; // add "__proto__" for Object.getPrototypeOf polyfill

    result[IE_PROTO] = O;
  } else result = NullProtoObject();

  return Properties === undefined ? result : objectDefineProperties(result, Properties);
};

var hiddenKeys$1 = enumBugKeys.concat('length', 'prototype'); // `Object.getOwnPropertyNames` method
// https://tc39.github.io/ecma262/#sec-object.getownpropertynames

var f$3 = Object.getOwnPropertyNames || function getOwnPropertyNames(O) {
  return objectKeysInternal(O, hiddenKeys$1);
};

var objectGetOwnPropertyNames = {
  f: f$3
};

var nativeGetOwnPropertyNames = objectGetOwnPropertyNames.f;
var toString$1 = {}.toString;
var windowNames = typeof window == 'object' && window && Object.getOwnPropertyNames ? Object.getOwnPropertyNames(window) : [];

var getWindowNames = function (it) {
  try {
    return nativeGetOwnPropertyNames(it);
  } catch (error) {
    return windowNames.slice();
  }
}; // fallback for IE11 buggy Object.getOwnPropertyNames with iframe and window


var f$4 = function getOwnPropertyNames(it) {
  return windowNames && toString$1.call(it) == '[object Window]' ? getWindowNames(it) : nativeGetOwnPropertyNames(toIndexedObject(it));
};

var objectGetOwnPropertyNamesExternal = {
  f: f$4
};

var f$5 = Object.getOwnPropertySymbols;
var objectGetOwnPropertySymbols = {
  f: f$5
};

var redefine = function (target, key, value, options) {
  if (options && options.enumerable) target[key] = value;else createNonEnumerableProperty(target, key, value);
};

var WellKnownSymbolsStore = shared('wks');
var Symbol$1 = global_1.Symbol;
var createWellKnownSymbol = useSymbolAsUid ? Symbol$1 : Symbol$1 && Symbol$1.withoutSetter || uid;

var wellKnownSymbol = function (name) {
  if (!has(WellKnownSymbolsStore, name)) {
    if (nativeSymbol && has(Symbol$1, name)) WellKnownSymbolsStore[name] = Symbol$1[name];else WellKnownSymbolsStore[name] = createWellKnownSymbol('Symbol.' + name);
  }

  return WellKnownSymbolsStore[name];
};

var f$6 = wellKnownSymbol;
var wrappedWellKnownSymbol = {
  f: f$6
};

var defineProperty = objectDefineProperty.f;

var defineWellKnownSymbol = function (NAME) {
  var Symbol = path.Symbol || (path.Symbol = {});
  if (!has(Symbol, NAME)) defineProperty(Symbol, NAME, {
    value: wrappedWellKnownSymbol.f(NAME)
  });
};

var TO_STRING_TAG = wellKnownSymbol('toStringTag');
var test = {};
test[TO_STRING_TAG] = 'z';
var toStringTagSupport = String(test) === '[object z]';

var TO_STRING_TAG$1 = wellKnownSymbol('toStringTag'); // ES3 wrong here

var CORRECT_ARGUMENTS = classofRaw(function () {
  return arguments;
}()) == 'Arguments'; // fallback for IE11 Script Access Denied error

var tryGet = function (it, key) {
  try {
    return it[key];
  } catch (error) {
    /* empty */
  }
}; // getting tag from ES6+ `Object.prototype.toString`


var classof = toStringTagSupport ? classofRaw : function (it) {
  var O, tag, result;
  return it === undefined ? 'Undefined' : it === null ? 'Null' // @@toStringTag case
  : typeof (tag = tryGet(O = Object(it), TO_STRING_TAG$1)) == 'string' ? tag // builtinTag case
  : CORRECT_ARGUMENTS ? classofRaw(O) // ES3 arguments fallback
  : (result = classofRaw(O)) == 'Object' && typeof O.callee == 'function' ? 'Arguments' : result;
};

// https://tc39.github.io/ecma262/#sec-object.prototype.tostring


var objectToString = toStringTagSupport ? {}.toString : function toString() {
  return '[object ' + classof(this) + ']';
};

var defineProperty$1 = objectDefineProperty.f;
var TO_STRING_TAG$2 = wellKnownSymbol('toStringTag');

var setToStringTag = function (it, TAG, STATIC, SET_METHOD) {
  if (it) {
    var target = STATIC ? it : it.prototype;

    if (!has(target, TO_STRING_TAG$2)) {
      defineProperty$1(target, TO_STRING_TAG$2, {
        configurable: true,
        value: TAG
      });
    }

    if (SET_METHOD && !toStringTagSupport) {
      createNonEnumerableProperty(target, 'toString', objectToString);
    }
  }
};

var functionToString = Function.toString; // this helper broken in `3.4.1-3.4.4`, so we can't use `shared` helper

if (typeof sharedStore.inspectSource != 'function') {
  sharedStore.inspectSource = function (it) {
    return functionToString.call(it);
  };
}

var inspectSource = sharedStore.inspectSource;

var WeakMap$1 = global_1.WeakMap;
var nativeWeakMap = typeof WeakMap$1 === 'function' && /native code/.test(inspectSource(WeakMap$1));

var WeakMap$2 = global_1.WeakMap;
var set, get, has$1;

var enforce = function (it) {
  return has$1(it) ? get(it) : set(it, {});
};

var getterFor = function (TYPE) {
  return function (it) {
    var state;

    if (!isObject(it) || (state = get(it)).type !== TYPE) {
      throw TypeError('Incompatible receiver, ' + TYPE + ' required');
    }

    return state;
  };
};

if (nativeWeakMap) {
  var store$1 = new WeakMap$2();
  var wmget = store$1.get;
  var wmhas = store$1.has;
  var wmset = store$1.set;

  set = function (it, metadata) {
    wmset.call(store$1, it, metadata);
    return metadata;
  };

  get = function (it) {
    return wmget.call(store$1, it) || {};
  };

  has$1 = function (it) {
    return wmhas.call(store$1, it);
  };
} else {
  var STATE = sharedKey('state');
  hiddenKeys[STATE] = true;

  set = function (it, metadata) {
    createNonEnumerableProperty(it, STATE, metadata);
    return metadata;
  };

  get = function (it) {
    return has(it, STATE) ? it[STATE] : {};
  };

  has$1 = function (it) {
    return has(it, STATE);
  };
}

var internalState = {
  set: set,
  get: get,
  has: has$1,
  enforce: enforce,
  getterFor: getterFor
};

var SPECIES = wellKnownSymbol('species'); // `ArraySpeciesCreate` abstract operation
// https://tc39.github.io/ecma262/#sec-arrayspeciescreate

var arraySpeciesCreate = function (originalArray, length) {
  var C;

  if (isArray(originalArray)) {
    C = originalArray.constructor; // cross-realm fallback

    if (typeof C == 'function' && (C === Array || isArray(C.prototype))) C = undefined;else if (isObject(C)) {
      C = C[SPECIES];
      if (C === null) C = undefined;
    }
  }

  return new (C === undefined ? Array : C)(length === 0 ? 0 : length);
};

var push = [].push; // `Array.prototype.{ forEach, map, filter, some, every, find, findIndex }` methods implementation

var createMethod$1 = function (TYPE) {
  var IS_MAP = TYPE == 1;
  var IS_FILTER = TYPE == 2;
  var IS_SOME = TYPE == 3;
  var IS_EVERY = TYPE == 4;
  var IS_FIND_INDEX = TYPE == 6;
  var NO_HOLES = TYPE == 5 || IS_FIND_INDEX;
  return function ($this, callbackfn, that, specificCreate) {
    var O = toObject($this);
    var self = indexedObject(O);
    var boundFunction = bindContext(callbackfn, that, 3);
    var length = toLength(self.length);
    var index = 0;
    var create = specificCreate || arraySpeciesCreate;
    var target = IS_MAP ? create($this, length) : IS_FILTER ? create($this, 0) : undefined;
    var value, result;

    for (; length > index; index++) if (NO_HOLES || index in self) {
      value = self[index];
      result = boundFunction(value, index, O);

      if (TYPE) {
        if (IS_MAP) target[index] = result; // map
        else if (result) switch (TYPE) {
            case 3:
              return true;
            // some

            case 5:
              return value;
            // find

            case 6:
              return index;
            // findIndex

            case 2:
              push.call(target, value);
            // filter
          } else if (IS_EVERY) return false; // every
      }
    }

    return IS_FIND_INDEX ? -1 : IS_SOME || IS_EVERY ? IS_EVERY : target;
  };
};

var arrayIteration = {
  // `Array.prototype.forEach` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.foreach
  forEach: createMethod$1(0),
  // `Array.prototype.map` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.map
  map: createMethod$1(1),
  // `Array.prototype.filter` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.filter
  filter: createMethod$1(2),
  // `Array.prototype.some` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.some
  some: createMethod$1(3),
  // `Array.prototype.every` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.every
  every: createMethod$1(4),
  // `Array.prototype.find` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.find
  find: createMethod$1(5),
  // `Array.prototype.findIndex` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.findIndex
  findIndex: createMethod$1(6)
};

var $forEach = arrayIteration.forEach;
var HIDDEN = sharedKey('hidden');
var SYMBOL = 'Symbol';
var PROTOTYPE$1 = 'prototype';
var TO_PRIMITIVE = wellKnownSymbol('toPrimitive');
var setInternalState = internalState.set;
var getInternalState = internalState.getterFor(SYMBOL);
var ObjectPrototype = Object[PROTOTYPE$1];
var $Symbol = global_1.Symbol;
var $stringify = getBuiltIn('JSON', 'stringify');
var nativeGetOwnPropertyDescriptor$1 = objectGetOwnPropertyDescriptor.f;
var nativeDefineProperty$1 = objectDefineProperty.f;
var nativeGetOwnPropertyNames$1 = objectGetOwnPropertyNamesExternal.f;
var nativePropertyIsEnumerable$1 = objectPropertyIsEnumerable.f;
var AllSymbols = shared('symbols');
var ObjectPrototypeSymbols = shared('op-symbols');
var StringToSymbolRegistry = shared('string-to-symbol-registry');
var SymbolToStringRegistry = shared('symbol-to-string-registry');
var WellKnownSymbolsStore$1 = shared('wks');
var QObject = global_1.QObject; // Don't use setters in Qt Script, https://github.com/zloirock/core-js/issues/173

var USE_SETTER = !QObject || !QObject[PROTOTYPE$1] || !QObject[PROTOTYPE$1].findChild; // fallback for old Android, https://code.google.com/p/v8/issues/detail?id=687

var setSymbolDescriptor = descriptors && fails(function () {
  return objectCreate(nativeDefineProperty$1({}, 'a', {
    get: function () {
      return nativeDefineProperty$1(this, 'a', {
        value: 7
      }).a;
    }
  })).a != 7;
}) ? function (O, P, Attributes) {
  var ObjectPrototypeDescriptor = nativeGetOwnPropertyDescriptor$1(ObjectPrototype, P);
  if (ObjectPrototypeDescriptor) delete ObjectPrototype[P];
  nativeDefineProperty$1(O, P, Attributes);

  if (ObjectPrototypeDescriptor && O !== ObjectPrototype) {
    nativeDefineProperty$1(ObjectPrototype, P, ObjectPrototypeDescriptor);
  }
} : nativeDefineProperty$1;

var wrap = function (tag, description) {
  var symbol = AllSymbols[tag] = objectCreate($Symbol[PROTOTYPE$1]);
  setInternalState(symbol, {
    type: SYMBOL,
    tag: tag,
    description: description
  });
  if (!descriptors) symbol.description = description;
  return symbol;
};

var isSymbol = useSymbolAsUid ? function (it) {
  return typeof it == 'symbol';
} : function (it) {
  return Object(it) instanceof $Symbol;
};

var $defineProperty = function defineProperty(O, P, Attributes) {
  if (O === ObjectPrototype) $defineProperty(ObjectPrototypeSymbols, P, Attributes);
  anObject(O);
  var key = toPrimitive(P, true);
  anObject(Attributes);

  if (has(AllSymbols, key)) {
    if (!Attributes.enumerable) {
      if (!has(O, HIDDEN)) nativeDefineProperty$1(O, HIDDEN, createPropertyDescriptor(1, {}));
      O[HIDDEN][key] = true;
    } else {
      if (has(O, HIDDEN) && O[HIDDEN][key]) O[HIDDEN][key] = false;
      Attributes = objectCreate(Attributes, {
        enumerable: createPropertyDescriptor(0, false)
      });
    }

    return setSymbolDescriptor(O, key, Attributes);
  }

  return nativeDefineProperty$1(O, key, Attributes);
};

var $defineProperties = function defineProperties(O, Properties) {
  anObject(O);
  var properties = toIndexedObject(Properties);
  var keys = objectKeys(properties).concat($getOwnPropertySymbols(properties));
  $forEach(keys, function (key) {
    if (!descriptors || $propertyIsEnumerable.call(properties, key)) $defineProperty(O, key, properties[key]);
  });
  return O;
};

var $create = function create(O, Properties) {
  return Properties === undefined ? objectCreate(O) : $defineProperties(objectCreate(O), Properties);
};

var $propertyIsEnumerable = function propertyIsEnumerable(V) {
  var P = toPrimitive(V, true);
  var enumerable = nativePropertyIsEnumerable$1.call(this, P);
  if (this === ObjectPrototype && has(AllSymbols, P) && !has(ObjectPrototypeSymbols, P)) return false;
  return enumerable || !has(this, P) || !has(AllSymbols, P) || has(this, HIDDEN) && this[HIDDEN][P] ? enumerable : true;
};

var $getOwnPropertyDescriptor = function getOwnPropertyDescriptor(O, P) {
  var it = toIndexedObject(O);
  var key = toPrimitive(P, true);
  if (it === ObjectPrototype && has(AllSymbols, key) && !has(ObjectPrototypeSymbols, key)) return;
  var descriptor = nativeGetOwnPropertyDescriptor$1(it, key);

  if (descriptor && has(AllSymbols, key) && !(has(it, HIDDEN) && it[HIDDEN][key])) {
    descriptor.enumerable = true;
  }

  return descriptor;
};

var $getOwnPropertyNames = function getOwnPropertyNames(O) {
  var names = nativeGetOwnPropertyNames$1(toIndexedObject(O));
  var result = [];
  $forEach(names, function (key) {
    if (!has(AllSymbols, key) && !has(hiddenKeys, key)) result.push(key);
  });
  return result;
};

var $getOwnPropertySymbols = function getOwnPropertySymbols(O) {
  var IS_OBJECT_PROTOTYPE = O === ObjectPrototype;
  var names = nativeGetOwnPropertyNames$1(IS_OBJECT_PROTOTYPE ? ObjectPrototypeSymbols : toIndexedObject(O));
  var result = [];
  $forEach(names, function (key) {
    if (has(AllSymbols, key) && (!IS_OBJECT_PROTOTYPE || has(ObjectPrototype, key))) {
      result.push(AllSymbols[key]);
    }
  });
  return result;
}; // `Symbol` constructor
// https://tc39.github.io/ecma262/#sec-symbol-constructor


if (!nativeSymbol) {
  $Symbol = function Symbol() {
    if (this instanceof $Symbol) throw TypeError('Symbol is not a constructor');
    var description = !arguments.length || arguments[0] === undefined ? undefined : String(arguments[0]);
    var tag = uid(description);

    var setter = function (value) {
      if (this === ObjectPrototype) setter.call(ObjectPrototypeSymbols, value);
      if (has(this, HIDDEN) && has(this[HIDDEN], tag)) this[HIDDEN][tag] = false;
      setSymbolDescriptor(this, tag, createPropertyDescriptor(1, value));
    };

    if (descriptors && USE_SETTER) setSymbolDescriptor(ObjectPrototype, tag, {
      configurable: true,
      set: setter
    });
    return wrap(tag, description);
  };

  redefine($Symbol[PROTOTYPE$1], 'toString', function toString() {
    return getInternalState(this).tag;
  });
  redefine($Symbol, 'withoutSetter', function (description) {
    return wrap(uid(description), description);
  });
  objectPropertyIsEnumerable.f = $propertyIsEnumerable;
  objectDefineProperty.f = $defineProperty;
  objectGetOwnPropertyDescriptor.f = $getOwnPropertyDescriptor;
  objectGetOwnPropertyNames.f = objectGetOwnPropertyNamesExternal.f = $getOwnPropertyNames;
  objectGetOwnPropertySymbols.f = $getOwnPropertySymbols;

  wrappedWellKnownSymbol.f = function (name) {
    return wrap(wellKnownSymbol(name), name);
  };

  if (descriptors) {
    // https://github.com/tc39/proposal-Symbol-description
    nativeDefineProperty$1($Symbol[PROTOTYPE$1], 'description', {
      configurable: true,
      get: function description() {
        return getInternalState(this).description;
      }
    });
  }
}

_export({
  global: true,
  wrap: true,
  forced: !nativeSymbol,
  sham: !nativeSymbol
}, {
  Symbol: $Symbol
});
$forEach(objectKeys(WellKnownSymbolsStore$1), function (name) {
  defineWellKnownSymbol(name);
});
_export({
  target: SYMBOL,
  stat: true,
  forced: !nativeSymbol
}, {
  // `Symbol.for` method
  // https://tc39.github.io/ecma262/#sec-symbol.for
  'for': function (key) {
    var string = String(key);
    if (has(StringToSymbolRegistry, string)) return StringToSymbolRegistry[string];
    var symbol = $Symbol(string);
    StringToSymbolRegistry[string] = symbol;
    SymbolToStringRegistry[symbol] = string;
    return symbol;
  },
  // `Symbol.keyFor` method
  // https://tc39.github.io/ecma262/#sec-symbol.keyfor
  keyFor: function keyFor(sym) {
    if (!isSymbol(sym)) throw TypeError(sym + ' is not a symbol');
    if (has(SymbolToStringRegistry, sym)) return SymbolToStringRegistry[sym];
  },
  useSetter: function () {
    USE_SETTER = true;
  },
  useSimple: function () {
    USE_SETTER = false;
  }
});
_export({
  target: 'Object',
  stat: true,
  forced: !nativeSymbol,
  sham: !descriptors
}, {
  // `Object.create` method
  // https://tc39.github.io/ecma262/#sec-object.create
  create: $create,
  // `Object.defineProperty` method
  // https://tc39.github.io/ecma262/#sec-object.defineproperty
  defineProperty: $defineProperty,
  // `Object.defineProperties` method
  // https://tc39.github.io/ecma262/#sec-object.defineproperties
  defineProperties: $defineProperties,
  // `Object.getOwnPropertyDescriptor` method
  // https://tc39.github.io/ecma262/#sec-object.getownpropertydescriptors
  getOwnPropertyDescriptor: $getOwnPropertyDescriptor
});
_export({
  target: 'Object',
  stat: true,
  forced: !nativeSymbol
}, {
  // `Object.getOwnPropertyNames` method
  // https://tc39.github.io/ecma262/#sec-object.getownpropertynames
  getOwnPropertyNames: $getOwnPropertyNames,
  // `Object.getOwnPropertySymbols` method
  // https://tc39.github.io/ecma262/#sec-object.getownpropertysymbols
  getOwnPropertySymbols: $getOwnPropertySymbols
}); // Chrome 38 and 39 `Object.getOwnPropertySymbols` fails on primitives
// https://bugs.chromium.org/p/v8/issues/detail?id=3443

_export({
  target: 'Object',
  stat: true,
  forced: fails(function () {
    objectGetOwnPropertySymbols.f(1);
  })
}, {
  getOwnPropertySymbols: function getOwnPropertySymbols(it) {
    return objectGetOwnPropertySymbols.f(toObject(it));
  }
}); // `JSON.stringify` method behavior with symbols
// https://tc39.github.io/ecma262/#sec-json.stringify

if ($stringify) {
  var FORCED_JSON_STRINGIFY = !nativeSymbol || fails(function () {
    var symbol = $Symbol(); // MS Edge converts symbol values to JSON as {}

    return $stringify([symbol]) != '[null]' // WebKit converts symbol values to JSON as null
    || $stringify({
      a: symbol
    }) != '{}' // V8 throws on boxed symbols
    || $stringify(Object(symbol)) != '{}';
  });
  _export({
    target: 'JSON',
    stat: true,
    forced: FORCED_JSON_STRINGIFY
  }, {
    // eslint-disable-next-line no-unused-vars
    stringify: function stringify(it, replacer, space) {
      var args = [it];
      var index = 1;
      var $replacer;

      while (arguments.length > index) args.push(arguments[index++]);

      $replacer = replacer;
      if (!isObject(replacer) && it === undefined || isSymbol(it)) return; // IE8 returns string on undefined

      if (!isArray(replacer)) replacer = function (key, value) {
        if (typeof $replacer == 'function') value = $replacer.call(this, key, value);
        if (!isSymbol(value)) return value;
      };
      args[1] = replacer;
      return $stringify.apply(null, args);
    }
  });
} // `Symbol.prototype[@@toPrimitive]` method
// https://tc39.github.io/ecma262/#sec-symbol.prototype-@@toprimitive


if (!$Symbol[PROTOTYPE$1][TO_PRIMITIVE]) {
  createNonEnumerableProperty($Symbol[PROTOTYPE$1], TO_PRIMITIVE, $Symbol[PROTOTYPE$1].valueOf);
} // `Symbol.prototype[@@toStringTag]` property
// https://tc39.github.io/ecma262/#sec-symbol.prototype-@@tostringtag


setToStringTag($Symbol, SYMBOL);
hiddenKeys[HIDDEN] = true;

var _for = path.Symbol['for'];

var _for$1 = _for;

var _for$2 = _for$1;

var metadata_keys = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.NAMED_TAG = "named";
  exports.NAME_TAG = "name";
  exports.UNMANAGED_TAG = "unmanaged";
  exports.OPTIONAL_TAG = "optional";
  exports.INJECT_TAG = "inject";
  exports.MULTI_INJECT_TAG = "multi_inject";
  exports.TAGGED = "inversify:tagged";
  exports.TAGGED_PROP = "inversify:tagged_props";
  exports.PARAM_TYPES = "inversify:paramtypes";
  exports.DESIGN_PARAM_TYPES = "design:paramtypes";
  exports.POST_CONSTRUCT = "post_construct";
});
unwrapExports(metadata_keys);
var metadata_keys_1 = metadata_keys.NAMED_TAG;
var metadata_keys_2 = metadata_keys.NAME_TAG;
var metadata_keys_3 = metadata_keys.UNMANAGED_TAG;
var metadata_keys_4 = metadata_keys.OPTIONAL_TAG;
var metadata_keys_5 = metadata_keys.INJECT_TAG;
var metadata_keys_6 = metadata_keys.MULTI_INJECT_TAG;
var metadata_keys_7 = metadata_keys.TAGGED;
var metadata_keys_8 = metadata_keys.TAGGED_PROP;
var metadata_keys_9 = metadata_keys.PARAM_TYPES;
var metadata_keys_10 = metadata_keys.DESIGN_PARAM_TYPES;
var metadata_keys_11 = metadata_keys.POST_CONSTRUCT;

var literal_types = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  var BindingScopeEnum = {
    Request: "Request",
    Singleton: "Singleton",
    Transient: "Transient"
  };
  exports.BindingScopeEnum = BindingScopeEnum;
  var BindingTypeEnum = {
    ConstantValue: "ConstantValue",
    Constructor: "Constructor",
    DynamicValue: "DynamicValue",
    Factory: "Factory",
    Function: "Function",
    Instance: "Instance",
    Invalid: "Invalid",
    Provider: "Provider"
  };
  exports.BindingTypeEnum = BindingTypeEnum;
  var TargetTypeEnum = {
    ClassProperty: "ClassProperty",
    ConstructorArgument: "ConstructorArgument",
    Variable: "Variable"
  };
  exports.TargetTypeEnum = TargetTypeEnum;
});
unwrapExports(literal_types);
var literal_types_1 = literal_types.BindingScopeEnum;
var literal_types_2 = literal_types.BindingTypeEnum;
var literal_types_3 = literal_types.TargetTypeEnum;

var id_1 = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  var idCounter = 0;

  function id() {
    return idCounter++;
  }

  exports.id = id;
});
unwrapExports(id_1);
var id_2 = id_1.id;

var binding = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var Binding = function () {
    function Binding(serviceIdentifier, scope) {
      this.id = id_1.id();
      this.activated = false;
      this.serviceIdentifier = serviceIdentifier;
      this.scope = scope;
      this.type = literal_types.BindingTypeEnum.Invalid;

      this.constraint = function (request) {
        return true;
      };

      this.implementationType = null;
      this.cache = null;
      this.factory = null;
      this.provider = null;
      this.onActivation = null;
      this.dynamicValue = null;
    }

    Binding.prototype.clone = function () {
      var clone = new Binding(this.serviceIdentifier, this.scope);
      clone.activated = false;
      clone.implementationType = this.implementationType;
      clone.dynamicValue = this.dynamicValue;
      clone.scope = this.scope;
      clone.type = this.type;
      clone.factory = this.factory;
      clone.provider = this.provider;
      clone.constraint = this.constraint;
      clone.onActivation = this.onActivation;
      clone.cache = this.cache;
      return clone;
    };

    return Binding;
  }();

  exports.Binding = Binding;
});
unwrapExports(binding);
var binding_1 = binding.Binding;

var error_msgs = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.DUPLICATED_INJECTABLE_DECORATOR = "Cannot apply @injectable decorator multiple times.";
  exports.DUPLICATED_METADATA = "Metadata key was used more than once in a parameter:";
  exports.NULL_ARGUMENT = "NULL argument";
  exports.KEY_NOT_FOUND = "Key Not Found";
  exports.AMBIGUOUS_MATCH = "Ambiguous match found for serviceIdentifier:";
  exports.CANNOT_UNBIND = "Could not unbind serviceIdentifier:";
  exports.NOT_REGISTERED = "No matching bindings found for serviceIdentifier:";
  exports.MISSING_INJECTABLE_ANNOTATION = "Missing required @injectable annotation in:";
  exports.MISSING_INJECT_ANNOTATION = "Missing required @inject or @multiInject annotation in:";

  exports.UNDEFINED_INJECT_ANNOTATION = function (name) {
    return "@inject called with undefined this could mean that the class " + name + " has " + "a circular dependency problem. You can use a LazyServiceIdentifer to  " + "overcome this limitation.";
  };

  exports.CIRCULAR_DEPENDENCY = "Circular dependency found:";
  exports.NOT_IMPLEMENTED = "Sorry, this feature is not fully implemented yet.";
  exports.INVALID_BINDING_TYPE = "Invalid binding type:";
  exports.NO_MORE_SNAPSHOTS_AVAILABLE = "No snapshot available to restore.";
  exports.INVALID_MIDDLEWARE_RETURN = "Invalid return type in middleware. Middleware must return!";
  exports.INVALID_FUNCTION_BINDING = "Value provided to function binding must be a function!";
  exports.INVALID_TO_SELF_VALUE = "The toSelf function can only be applied when a constructor is " + "used as service identifier";
  exports.INVALID_DECORATOR_OPERATION = "The @inject @multiInject @tagged and @named decorators " + "must be applied to the parameters of a class constructor or a class property.";

  exports.ARGUMENTS_LENGTH_MISMATCH = function () {
    var values = [];

    for (var _i = 0; _i < arguments.length; _i++) {
      values[_i] = arguments[_i];
    }

    return "The number of constructor arguments in the derived class " + (values[0] + " must be >= than the number of constructor arguments of its base class.");
  };

  exports.CONTAINER_OPTIONS_MUST_BE_AN_OBJECT = "Invalid Container constructor argument. Container options " + "must be an object.";
  exports.CONTAINER_OPTIONS_INVALID_DEFAULT_SCOPE = "Invalid Container option. Default scope must " + "be a string ('singleton' or 'transient').";
  exports.CONTAINER_OPTIONS_INVALID_AUTO_BIND_INJECTABLE = "Invalid Container option. Auto bind injectable must " + "be a boolean";
  exports.CONTAINER_OPTIONS_INVALID_SKIP_BASE_CHECK = "Invalid Container option. Skip base check must " + "be a boolean";
  exports.MULTIPLE_POST_CONSTRUCT_METHODS = "Cannot apply @postConstruct decorator multiple times in the same class";

  exports.POST_CONSTRUCT_ERROR = function () {
    var values = [];

    for (var _i = 0; _i < arguments.length; _i++) {
      values[_i] = arguments[_i];
    }

    return "@postConstruct error in class " + values[0] + ": " + values[1];
  };

  exports.CIRCULAR_DEPENDENCY_IN_FACTORY = function () {
    var values = [];

    for (var _i = 0; _i < arguments.length; _i++) {
      values[_i] = arguments[_i];
    }

    return "It looks like there is a circular dependency " + ("in one of the '" + values[0] + "' bindings. Please investigate bindings with") + ("service identifier '" + values[1] + "'.");
  };

  exports.STACK_OVERFLOW = "Maximum call stack size exceeded";
});
unwrapExports(error_msgs);
var error_msgs_1 = error_msgs.DUPLICATED_INJECTABLE_DECORATOR;
var error_msgs_2 = error_msgs.DUPLICATED_METADATA;
var error_msgs_3 = error_msgs.NULL_ARGUMENT;
var error_msgs_4 = error_msgs.KEY_NOT_FOUND;
var error_msgs_5 = error_msgs.AMBIGUOUS_MATCH;
var error_msgs_6 = error_msgs.CANNOT_UNBIND;
var error_msgs_7 = error_msgs.NOT_REGISTERED;
var error_msgs_8 = error_msgs.MISSING_INJECTABLE_ANNOTATION;
var error_msgs_9 = error_msgs.MISSING_INJECT_ANNOTATION;
var error_msgs_10 = error_msgs.UNDEFINED_INJECT_ANNOTATION;
var error_msgs_11 = error_msgs.CIRCULAR_DEPENDENCY;
var error_msgs_12 = error_msgs.NOT_IMPLEMENTED;
var error_msgs_13 = error_msgs.INVALID_BINDING_TYPE;
var error_msgs_14 = error_msgs.NO_MORE_SNAPSHOTS_AVAILABLE;
var error_msgs_15 = error_msgs.INVALID_MIDDLEWARE_RETURN;
var error_msgs_16 = error_msgs.INVALID_FUNCTION_BINDING;
var error_msgs_17 = error_msgs.INVALID_TO_SELF_VALUE;
var error_msgs_18 = error_msgs.INVALID_DECORATOR_OPERATION;
var error_msgs_19 = error_msgs.ARGUMENTS_LENGTH_MISMATCH;
var error_msgs_20 = error_msgs.CONTAINER_OPTIONS_MUST_BE_AN_OBJECT;
var error_msgs_21 = error_msgs.CONTAINER_OPTIONS_INVALID_DEFAULT_SCOPE;
var error_msgs_22 = error_msgs.CONTAINER_OPTIONS_INVALID_AUTO_BIND_INJECTABLE;
var error_msgs_23 = error_msgs.CONTAINER_OPTIONS_INVALID_SKIP_BASE_CHECK;
var error_msgs_24 = error_msgs.MULTIPLE_POST_CONSTRUCT_METHODS;
var error_msgs_25 = error_msgs.POST_CONSTRUCT_ERROR;
var error_msgs_26 = error_msgs.CIRCULAR_DEPENDENCY_IN_FACTORY;
var error_msgs_27 = error_msgs.STACK_OVERFLOW;

var metadata_reader = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var MetadataReader = function () {
    function MetadataReader() {}

    MetadataReader.prototype.getConstructorMetadata = function (constructorFunc) {
      var compilerGeneratedMetadata = Reflect.getMetadata(metadata_keys.PARAM_TYPES, constructorFunc);
      var userGeneratedMetadata = Reflect.getMetadata(metadata_keys.TAGGED, constructorFunc);
      return {
        compilerGeneratedMetadata: compilerGeneratedMetadata,
        userGeneratedMetadata: userGeneratedMetadata || {}
      };
    };

    MetadataReader.prototype.getPropertiesMetadata = function (constructorFunc) {
      var userGeneratedMetadata = Reflect.getMetadata(metadata_keys.TAGGED_PROP, constructorFunc) || [];
      return userGeneratedMetadata;
    };

    return MetadataReader;
  }();

  exports.MetadataReader = MetadataReader;
});
unwrapExports(metadata_reader);
var metadata_reader_1 = metadata_reader.MetadataReader;

var binding_count = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  var BindingCount = {
    MultipleBindingsAvailable: 2,
    NoBindingsAvailable: 0,
    OnlyOneBindingAvailable: 1
  };
  exports.BindingCount = BindingCount;
});
unwrapExports(binding_count);
var binding_count_1 = binding_count.BindingCount;

var exceptions = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function isStackOverflowExeption(error) {
    return error instanceof RangeError || error.message === error_msgs.STACK_OVERFLOW;
  }

  exports.isStackOverflowExeption = isStackOverflowExeption;
});
unwrapExports(exceptions);
var exceptions_1 = exceptions.isStackOverflowExeption;

var serialization = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function getServiceIdentifierAsString(serviceIdentifier) {
    if (typeof serviceIdentifier === "function") {
      var _serviceIdentifier = serviceIdentifier;
      return _serviceIdentifier.name;
    } else if (typeof serviceIdentifier === "symbol") {
      return serviceIdentifier.toString();
    } else {
      var _serviceIdentifier = serviceIdentifier;
      return _serviceIdentifier;
    }
  }

  exports.getServiceIdentifierAsString = getServiceIdentifierAsString;

  function listRegisteredBindingsForServiceIdentifier(container, serviceIdentifier, getBindings) {
    var registeredBindingsList = "";
    var registeredBindings = getBindings(container, serviceIdentifier);

    if (registeredBindings.length !== 0) {
      registeredBindingsList = "\nRegistered bindings:";
      registeredBindings.forEach(function (binding) {
        var name = "Object";

        if (binding.implementationType !== null) {
          name = getFunctionName(binding.implementationType);
        }

        registeredBindingsList = registeredBindingsList + "\n " + name;

        if (binding.constraint.metaData) {
          registeredBindingsList = registeredBindingsList + " - " + binding.constraint.metaData;
        }
      });
    }

    return registeredBindingsList;
  }

  exports.listRegisteredBindingsForServiceIdentifier = listRegisteredBindingsForServiceIdentifier;

  function alreadyDependencyChain(request, serviceIdentifier) {
    if (request.parentRequest === null) {
      return false;
    } else if (request.parentRequest.serviceIdentifier === serviceIdentifier) {
      return true;
    } else {
      return alreadyDependencyChain(request.parentRequest, serviceIdentifier);
    }
  }

  function dependencyChainToString(request) {
    function _createStringArr(req, result) {
      if (result === void 0) {
        result = [];
      }

      var serviceIdentifier = getServiceIdentifierAsString(req.serviceIdentifier);
      result.push(serviceIdentifier);

      if (req.parentRequest !== null) {
        return _createStringArr(req.parentRequest, result);
      }

      return result;
    }

    var stringArr = _createStringArr(request);

    return stringArr.reverse().join(" --> ");
  }

  function circularDependencyToException(request) {
    request.childRequests.forEach(function (childRequest) {
      if (alreadyDependencyChain(childRequest, childRequest.serviceIdentifier)) {
        var services = dependencyChainToString(childRequest);
        throw new Error(error_msgs.CIRCULAR_DEPENDENCY + " " + services);
      } else {
        circularDependencyToException(childRequest);
      }
    });
  }

  exports.circularDependencyToException = circularDependencyToException;

  function listMetadataForTarget(serviceIdentifierString, target) {
    if (target.isTagged() || target.isNamed()) {
      var m_1 = "";
      var namedTag = target.getNamedTag();
      var otherTags = target.getCustomTags();

      if (namedTag !== null) {
        m_1 += namedTag.toString() + "\n";
      }

      if (otherTags !== null) {
        otherTags.forEach(function (tag) {
          m_1 += tag.toString() + "\n";
        });
      }

      return " " + serviceIdentifierString + "\n " + serviceIdentifierString + " - " + m_1;
    } else {
      return " " + serviceIdentifierString;
    }
  }

  exports.listMetadataForTarget = listMetadataForTarget;

  function getFunctionName(v) {
    if (v.name) {
      return v.name;
    } else {
      var name_1 = v.toString();
      var match = name_1.match(/^function\s*([^\s(]+)/);
      return match ? match[1] : "Anonymous function: " + name_1;
    }
  }

  exports.getFunctionName = getFunctionName;
});
unwrapExports(serialization);
var serialization_1 = serialization.getServiceIdentifierAsString;
var serialization_2 = serialization.listRegisteredBindingsForServiceIdentifier;
var serialization_3 = serialization.circularDependencyToException;
var serialization_4 = serialization.listMetadataForTarget;
var serialization_5 = serialization.getFunctionName;

var context = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var Context = function () {
    function Context(container) {
      this.id = id_1.id();
      this.container = container;
    }

    Context.prototype.addPlan = function (plan) {
      this.plan = plan;
    };

    Context.prototype.setCurrentRequest = function (currentRequest) {
      this.currentRequest = currentRequest;
    };

    return Context;
  }();

  exports.Context = Context;
});
unwrapExports(context);
var context_1 = context.Context;

var metadata = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var Metadata = function () {
    function Metadata(key, value) {
      this.key = key;
      this.value = value;
    }

    Metadata.prototype.toString = function () {
      if (this.key === metadata_keys.NAMED_TAG) {
        return "named: " + this.value.toString() + " ";
      } else {
        return "tagged: { key:" + this.key.toString() + ", value: " + this.value + " }";
      }
    };

    return Metadata;
  }();

  exports.Metadata = Metadata;
});
unwrapExports(metadata);
var metadata_1 = metadata.Metadata;

var plan = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var Plan = function () {
    function Plan(parentContext, rootRequest) {
      this.parentContext = parentContext;
      this.rootRequest = rootRequest;
    }

    return Plan;
  }();

  exports.Plan = Plan;
});
unwrapExports(plan);
var plan_1 = plan.Plan;

var decorator_utils = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function tagParameter(annotationTarget, propertyName, parameterIndex, metadata) {
    var metadataKey = metadata_keys.TAGGED;

    _tagParameterOrProperty(metadataKey, annotationTarget, propertyName, metadata, parameterIndex);
  }

  exports.tagParameter = tagParameter;

  function tagProperty(annotationTarget, propertyName, metadata) {
    var metadataKey = metadata_keys.TAGGED_PROP;

    _tagParameterOrProperty(metadataKey, annotationTarget.constructor, propertyName, metadata);
  }

  exports.tagProperty = tagProperty;

  function _tagParameterOrProperty(metadataKey, annotationTarget, propertyName, metadata, parameterIndex) {
    var paramsOrPropertiesMetadata = {};
    var isParameterDecorator = typeof parameterIndex === "number";
    var key = parameterIndex !== undefined && isParameterDecorator ? parameterIndex.toString() : propertyName;

    if (isParameterDecorator && propertyName !== undefined) {
      throw new Error(error_msgs.INVALID_DECORATOR_OPERATION);
    }

    if (Reflect.hasOwnMetadata(metadataKey, annotationTarget)) {
      paramsOrPropertiesMetadata = Reflect.getMetadata(metadataKey, annotationTarget);
    }

    var paramOrPropertyMetadata = paramsOrPropertiesMetadata[key];

    if (!Array.isArray(paramOrPropertyMetadata)) {
      paramOrPropertyMetadata = [];
    } else {
      for (var _i = 0, paramOrPropertyMetadata_1 = paramOrPropertyMetadata; _i < paramOrPropertyMetadata_1.length; _i++) {
        var m = paramOrPropertyMetadata_1[_i];

        if (m.key === metadata.key) {
          throw new Error(error_msgs.DUPLICATED_METADATA + " " + m.key.toString());
        }
      }
    }

    paramOrPropertyMetadata.push(metadata);
    paramsOrPropertiesMetadata[key] = paramOrPropertyMetadata;
    Reflect.defineMetadata(metadataKey, paramsOrPropertiesMetadata, annotationTarget);
  }

  function _decorate(decorators, target) {
    Reflect.decorate(decorators, target);
  }

  function _param(paramIndex, decorator) {
    return function (target, key) {
      decorator(target, key, paramIndex);
    };
  }

  function decorate(decorator, target, parameterIndex) {
    if (typeof parameterIndex === "number") {
      _decorate([_param(parameterIndex, decorator)], target);
    } else if (typeof parameterIndex === "string") {
      Reflect.decorate([decorator], target, parameterIndex);
    } else {
      _decorate([decorator], target);
    }
  }

  exports.decorate = decorate;
});
unwrapExports(decorator_utils);
var decorator_utils_1 = decorator_utils.tagParameter;
var decorator_utils_2 = decorator_utils.tagProperty;
var decorator_utils_3 = decorator_utils.decorate;

var inject_1 = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var LazyServiceIdentifer = function () {
    function LazyServiceIdentifer(cb) {
      this._cb = cb;
    }

    LazyServiceIdentifer.prototype.unwrap = function () {
      return this._cb();
    };

    return LazyServiceIdentifer;
  }();

  exports.LazyServiceIdentifer = LazyServiceIdentifer;

  function inject(serviceIdentifier) {
    return function (target, targetKey, index) {
      if (serviceIdentifier === undefined) {
        throw new Error(error_msgs.UNDEFINED_INJECT_ANNOTATION(target.name));
      }

      var metadata$1 = new metadata.Metadata(metadata_keys.INJECT_TAG, serviceIdentifier);

      if (typeof index === "number") {
        decorator_utils.tagParameter(target, targetKey, index, metadata$1);
      } else {
        decorator_utils.tagProperty(target, targetKey, metadata$1);
      }
    };
  }

  exports.inject = inject;
});
unwrapExports(inject_1);
var inject_2 = inject_1.LazyServiceIdentifer;
var inject_3 = inject_1.inject;

var queryable_string = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var QueryableString = function () {
    function QueryableString(str) {
      this.str = str;
    }

    QueryableString.prototype.startsWith = function (searchString) {
      return this.str.indexOf(searchString) === 0;
    };

    QueryableString.prototype.endsWith = function (searchString) {
      var reverseString = "";
      var reverseSearchString = searchString.split("").reverse().join("");
      reverseString = this.str.split("").reverse().join("");
      return this.startsWith.call({
        str: reverseString
      }, reverseSearchString);
    };

    QueryableString.prototype.contains = function (searchString) {
      return this.str.indexOf(searchString) !== -1;
    };

    QueryableString.prototype.equals = function (compareString) {
      return this.str === compareString;
    };

    QueryableString.prototype.value = function () {
      return this.str;
    };

    return QueryableString;
  }();

  exports.QueryableString = QueryableString;
});
unwrapExports(queryable_string);
var queryable_string_1 = queryable_string.QueryableString;

var target = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var Target = function () {
    function Target(type, name, serviceIdentifier, namedOrTagged) {
      this.id = id_1.id();
      this.type = type;
      this.serviceIdentifier = serviceIdentifier;
      this.name = new queryable_string.QueryableString(name || "");
      this.metadata = new Array();
      var metadataItem = null;

      if (typeof namedOrTagged === "string") {
        metadataItem = new metadata.Metadata(metadata_keys.NAMED_TAG, namedOrTagged);
      } else if (namedOrTagged instanceof metadata.Metadata) {
        metadataItem = namedOrTagged;
      }

      if (metadataItem !== null) {
        this.metadata.push(metadataItem);
      }
    }

    Target.prototype.hasTag = function (key) {
      for (var _i = 0, _a = this.metadata; _i < _a.length; _i++) {
        var m = _a[_i];

        if (m.key === key) {
          return true;
        }
      }

      return false;
    };

    Target.prototype.isArray = function () {
      return this.hasTag(metadata_keys.MULTI_INJECT_TAG);
    };

    Target.prototype.matchesArray = function (name) {
      return this.matchesTag(metadata_keys.MULTI_INJECT_TAG)(name);
    };

    Target.prototype.isNamed = function () {
      return this.hasTag(metadata_keys.NAMED_TAG);
    };

    Target.prototype.isTagged = function () {
      return this.metadata.some(function (m) {
        return m.key !== metadata_keys.INJECT_TAG && m.key !== metadata_keys.MULTI_INJECT_TAG && m.key !== metadata_keys.NAME_TAG && m.key !== metadata_keys.UNMANAGED_TAG && m.key !== metadata_keys.NAMED_TAG;
      });
    };

    Target.prototype.isOptional = function () {
      return this.matchesTag(metadata_keys.OPTIONAL_TAG)(true);
    };

    Target.prototype.getNamedTag = function () {
      if (this.isNamed()) {
        return this.metadata.filter(function (m) {
          return m.key === metadata_keys.NAMED_TAG;
        })[0];
      }

      return null;
    };

    Target.prototype.getCustomTags = function () {
      if (this.isTagged()) {
        return this.metadata.filter(function (m) {
          return m.key !== metadata_keys.INJECT_TAG && m.key !== metadata_keys.MULTI_INJECT_TAG && m.key !== metadata_keys.NAME_TAG && m.key !== metadata_keys.UNMANAGED_TAG && m.key !== metadata_keys.NAMED_TAG;
        });
      }

      return null;
    };

    Target.prototype.matchesNamedTag = function (name) {
      return this.matchesTag(metadata_keys.NAMED_TAG)(name);
    };

    Target.prototype.matchesTag = function (key) {
      var _this = this;

      return function (value) {
        for (var _i = 0, _a = _this.metadata; _i < _a.length; _i++) {
          var m = _a[_i];

          if (m.key === key && m.value === value) {
            return true;
          }
        }

        return false;
      };
    };

    return Target;
  }();

  exports.Target = Target;
});
unwrapExports(target);
var target_1 = target.Target;

var reflection_utils = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.getFunctionName = serialization.getFunctionName;

  function getDependencies(metadataReader, func) {
    var constructorName = serialization.getFunctionName(func);
    var targets = getTargets(metadataReader, constructorName, func, false);
    return targets;
  }

  exports.getDependencies = getDependencies;

  function getTargets(metadataReader, constructorName, func, isBaseClass) {
    var metadata = metadataReader.getConstructorMetadata(func);
    var serviceIdentifiers = metadata.compilerGeneratedMetadata;

    if (serviceIdentifiers === undefined) {
      var msg = error_msgs.MISSING_INJECTABLE_ANNOTATION + " " + constructorName + ".";
      throw new Error(msg);
    }

    var constructorArgsMetadata = metadata.userGeneratedMetadata;
    var keys = Object.keys(constructorArgsMetadata);
    var hasUserDeclaredUnknownInjections = func.length === 0 && keys.length > 0;
    var iterations = hasUserDeclaredUnknownInjections ? keys.length : func.length;
    var constructorTargets = getConstructorArgsAsTargets(isBaseClass, constructorName, serviceIdentifiers, constructorArgsMetadata, iterations);
    var propertyTargets = getClassPropsAsTargets(metadataReader, func);
    var targets = constructorTargets.concat(propertyTargets);
    return targets;
  }

  function getConstructorArgsAsTarget(index, isBaseClass, constructorName, serviceIdentifiers, constructorArgsMetadata) {
    var targetMetadata = constructorArgsMetadata[index.toString()] || [];
    var metadata = formatTargetMetadata(targetMetadata);
    var isManaged = metadata.unmanaged !== true;
    var serviceIdentifier = serviceIdentifiers[index];
    var injectIdentifier = metadata.inject || metadata.multiInject;
    serviceIdentifier = injectIdentifier ? injectIdentifier : serviceIdentifier;

    if (serviceIdentifier instanceof inject_1.LazyServiceIdentifer) {
      serviceIdentifier = serviceIdentifier.unwrap();
    }

    if (isManaged) {
      var isObject = serviceIdentifier === Object;
      var isFunction = serviceIdentifier === Function;
      var isUndefined = serviceIdentifier === undefined;
      var isUnknownType = isObject || isFunction || isUndefined;

      if (!isBaseClass && isUnknownType) {
        var msg = error_msgs.MISSING_INJECT_ANNOTATION + " argument " + index + " in class " + constructorName + ".";
        throw new Error(msg);
      }

      var target$1 = new target.Target(literal_types.TargetTypeEnum.ConstructorArgument, metadata.targetName, serviceIdentifier);
      target$1.metadata = targetMetadata;
      return target$1;
    }

    return null;
  }

  function getConstructorArgsAsTargets(isBaseClass, constructorName, serviceIdentifiers, constructorArgsMetadata, iterations) {
    var targets = [];

    for (var i = 0; i < iterations; i++) {
      var index = i;
      var target = getConstructorArgsAsTarget(index, isBaseClass, constructorName, serviceIdentifiers, constructorArgsMetadata);

      if (target !== null) {
        targets.push(target);
      }
    }

    return targets;
  }

  function getClassPropsAsTargets(metadataReader, constructorFunc) {
    var classPropsMetadata = metadataReader.getPropertiesMetadata(constructorFunc);
    var targets = [];
    var keys = Object.keys(classPropsMetadata);

    for (var _i = 0, keys_1 = keys; _i < keys_1.length; _i++) {
      var key = keys_1[_i];
      var targetMetadata = classPropsMetadata[key];
      var metadata = formatTargetMetadata(classPropsMetadata[key]);
      var targetName = metadata.targetName || key;
      var serviceIdentifier = metadata.inject || metadata.multiInject;
      var target$1 = new target.Target(literal_types.TargetTypeEnum.ClassProperty, targetName, serviceIdentifier);
      target$1.metadata = targetMetadata;
      targets.push(target$1);
    }

    var baseConstructor = Object.getPrototypeOf(constructorFunc.prototype).constructor;

    if (baseConstructor !== Object) {
      var baseTargets = getClassPropsAsTargets(metadataReader, baseConstructor);
      targets = targets.concat(baseTargets);
    }

    return targets;
  }

  function getBaseClassDependencyCount(metadataReader, func) {
    var baseConstructor = Object.getPrototypeOf(func.prototype).constructor;

    if (baseConstructor !== Object) {
      var baseConstructorName = serialization.getFunctionName(baseConstructor);
      var targets = getTargets(metadataReader, baseConstructorName, baseConstructor, true);
      var metadata = targets.map(function (t) {
        return t.metadata.filter(function (m) {
          return m.key === metadata_keys.UNMANAGED_TAG;
        });
      });
      var unmanagedCount = [].concat.apply([], metadata).length;
      var dependencyCount = targets.length - unmanagedCount;

      if (dependencyCount > 0) {
        return dependencyCount;
      } else {
        return getBaseClassDependencyCount(metadataReader, baseConstructor);
      }
    } else {
      return 0;
    }
  }

  exports.getBaseClassDependencyCount = getBaseClassDependencyCount;

  function formatTargetMetadata(targetMetadata) {
    var targetMetadataMap = {};
    targetMetadata.forEach(function (m) {
      targetMetadataMap[m.key.toString()] = m.value;
    });
    return {
      inject: targetMetadataMap[metadata_keys.INJECT_TAG],
      multiInject: targetMetadataMap[metadata_keys.MULTI_INJECT_TAG],
      targetName: targetMetadataMap[metadata_keys.NAME_TAG],
      unmanaged: targetMetadataMap[metadata_keys.UNMANAGED_TAG]
    };
  }
});
unwrapExports(reflection_utils);
var reflection_utils_1 = reflection_utils.getFunctionName;
var reflection_utils_2 = reflection_utils.getDependencies;
var reflection_utils_3 = reflection_utils.getBaseClassDependencyCount;

var request = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var Request = function () {
    function Request(serviceIdentifier, parentContext, parentRequest, bindings, target) {
      this.id = id_1.id();
      this.serviceIdentifier = serviceIdentifier;
      this.parentContext = parentContext;
      this.parentRequest = parentRequest;
      this.target = target;
      this.childRequests = [];
      this.bindings = Array.isArray(bindings) ? bindings : [bindings];
      this.requestScope = parentRequest === null ? new Map() : null;
    }

    Request.prototype.addChildRequest = function (serviceIdentifier, bindings, target) {
      var child = new Request(serviceIdentifier, this.parentContext, this, bindings, target);
      this.childRequests.push(child);
      return child;
    };

    return Request;
  }();

  exports.Request = Request;
});
unwrapExports(request);
var request_1 = request.Request;

var planner = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function getBindingDictionary(cntnr) {
    return cntnr._bindingDictionary;
  }

  exports.getBindingDictionary = getBindingDictionary;

  function _createTarget(isMultiInject, targetType, serviceIdentifier, name, key, value) {
    var metadataKey = isMultiInject ? metadata_keys.MULTI_INJECT_TAG : metadata_keys.INJECT_TAG;
    var injectMetadata = new metadata.Metadata(metadataKey, serviceIdentifier);
    var target$1 = new target.Target(targetType, name, serviceIdentifier, injectMetadata);

    if (key !== undefined) {
      var tagMetadata = new metadata.Metadata(key, value);
      target$1.metadata.push(tagMetadata);
    }

    return target$1;
  }

  function _getActiveBindings(metadataReader, avoidConstraints, context, parentRequest, target) {
    var bindings = getBindings(context.container, target.serviceIdentifier);
    var activeBindings = [];

    if (bindings.length === binding_count.BindingCount.NoBindingsAvailable && context.container.options.autoBindInjectable && typeof target.serviceIdentifier === "function" && metadataReader.getConstructorMetadata(target.serviceIdentifier).compilerGeneratedMetadata) {
      context.container.bind(target.serviceIdentifier).toSelf();
      bindings = getBindings(context.container, target.serviceIdentifier);
    }

    if (!avoidConstraints) {
      activeBindings = bindings.filter(function (binding) {
        var request$1 = new request.Request(binding.serviceIdentifier, context, parentRequest, binding, target);
        return binding.constraint(request$1);
      });
    } else {
      activeBindings = bindings;
    }

    _validateActiveBindingCount(target.serviceIdentifier, activeBindings, target, context.container);

    return activeBindings;
  }

  function _validateActiveBindingCount(serviceIdentifier, bindings, target, container) {
    switch (bindings.length) {
      case binding_count.BindingCount.NoBindingsAvailable:
        if (target.isOptional()) {
          return bindings;
        } else {
          var serviceIdentifierString = serialization.getServiceIdentifierAsString(serviceIdentifier);
          var msg = error_msgs.NOT_REGISTERED;
          msg += serialization.listMetadataForTarget(serviceIdentifierString, target);
          msg += serialization.listRegisteredBindingsForServiceIdentifier(container, serviceIdentifierString, getBindings);
          throw new Error(msg);
        }

      case binding_count.BindingCount.OnlyOneBindingAvailable:
        if (!target.isArray()) {
          return bindings;
        }

      case binding_count.BindingCount.MultipleBindingsAvailable:
      default:
        if (!target.isArray()) {
          var serviceIdentifierString = serialization.getServiceIdentifierAsString(serviceIdentifier);
          var msg = error_msgs.AMBIGUOUS_MATCH + " " + serviceIdentifierString;
          msg += serialization.listRegisteredBindingsForServiceIdentifier(container, serviceIdentifierString, getBindings);
          throw new Error(msg);
        } else {
          return bindings;
        }

    }
  }

  function _createSubRequests(metadataReader, avoidConstraints, serviceIdentifier, context, parentRequest, target) {
    var activeBindings;
    var childRequest;

    if (parentRequest === null) {
      activeBindings = _getActiveBindings(metadataReader, avoidConstraints, context, null, target);
      childRequest = new request.Request(serviceIdentifier, context, null, activeBindings, target);
      var thePlan = new plan.Plan(context, childRequest);
      context.addPlan(thePlan);
    } else {
      activeBindings = _getActiveBindings(metadataReader, avoidConstraints, context, parentRequest, target);
      childRequest = parentRequest.addChildRequest(target.serviceIdentifier, activeBindings, target);
    }

    activeBindings.forEach(function (binding) {
      var subChildRequest = null;

      if (target.isArray()) {
        subChildRequest = childRequest.addChildRequest(binding.serviceIdentifier, binding, target);
      } else {
        if (binding.cache) {
          return;
        }

        subChildRequest = childRequest;
      }

      if (binding.type === literal_types.BindingTypeEnum.Instance && binding.implementationType !== null) {
        var dependencies = reflection_utils.getDependencies(metadataReader, binding.implementationType);

        if (!context.container.options.skipBaseClassChecks) {
          var baseClassDependencyCount = reflection_utils.getBaseClassDependencyCount(metadataReader, binding.implementationType);

          if (dependencies.length < baseClassDependencyCount) {
            var error = error_msgs.ARGUMENTS_LENGTH_MISMATCH(reflection_utils.getFunctionName(binding.implementationType));
            throw new Error(error);
          }
        }

        dependencies.forEach(function (dependency) {
          _createSubRequests(metadataReader, false, dependency.serviceIdentifier, context, subChildRequest, dependency);
        });
      }
    });
  }

  function getBindings(container, serviceIdentifier) {
    var bindings = [];
    var bindingDictionary = getBindingDictionary(container);

    if (bindingDictionary.hasKey(serviceIdentifier)) {
      bindings = bindingDictionary.get(serviceIdentifier);
    } else if (container.parent !== null) {
      bindings = getBindings(container.parent, serviceIdentifier);
    }

    return bindings;
  }

  function plan$1(metadataReader, container, isMultiInject, targetType, serviceIdentifier, key, value, avoidConstraints) {
    if (avoidConstraints === void 0) {
      avoidConstraints = false;
    }

    var context$1 = new context.Context(container);

    var target = _createTarget(isMultiInject, targetType, serviceIdentifier, "", key, value);

    try {
      _createSubRequests(metadataReader, avoidConstraints, serviceIdentifier, context$1, null, target);

      return context$1;
    } catch (error) {
      if (exceptions.isStackOverflowExeption(error)) {
        if (context$1.plan) {
          serialization.circularDependencyToException(context$1.plan.rootRequest);
        }
      }

      throw error;
    }
  }

  exports.plan = plan$1;

  function createMockRequest(container, serviceIdentifier, key, value) {
    var target$1 = new target.Target(literal_types.TargetTypeEnum.Variable, "", serviceIdentifier, new metadata.Metadata(key, value));
    var context$1 = new context.Context(container);
    var request$1 = new request.Request(serviceIdentifier, context$1, null, [], target$1);
    return request$1;
  }

  exports.createMockRequest = createMockRequest;
});
unwrapExports(planner);
var planner_1 = planner.getBindingDictionary;
var planner_2 = planner.plan;
var planner_3 = planner.createMockRequest;

var instantiation = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function _injectProperties(instance, childRequests, resolveRequest) {
    var propertyInjectionsRequests = childRequests.filter(function (childRequest) {
      return childRequest.target !== null && childRequest.target.type === literal_types.TargetTypeEnum.ClassProperty;
    });
    var propertyInjections = propertyInjectionsRequests.map(resolveRequest);
    propertyInjectionsRequests.forEach(function (r, index) {
      var propertyName = "";
      propertyName = r.target.name.value();
      var injection = propertyInjections[index];
      instance[propertyName] = injection;
    });
    return instance;
  }

  function _createInstance(Func, injections) {
    return new (Func.bind.apply(Func, [void 0].concat(injections)))();
  }

  function _postConstruct(constr, result) {
    if (Reflect.hasMetadata(metadata_keys.POST_CONSTRUCT, constr)) {
      var data = Reflect.getMetadata(metadata_keys.POST_CONSTRUCT, constr);

      try {
        result[data.value]();
      } catch (e) {
        throw new Error(error_msgs.POST_CONSTRUCT_ERROR(constr.name, e.message));
      }
    }
  }

  function resolveInstance(constr, childRequests, resolveRequest) {
    var result = null;

    if (childRequests.length > 0) {
      var constructorInjectionsRequests = childRequests.filter(function (childRequest) {
        return childRequest.target !== null && childRequest.target.type === literal_types.TargetTypeEnum.ConstructorArgument;
      });
      var constructorInjections = constructorInjectionsRequests.map(resolveRequest);
      result = _createInstance(constr, constructorInjections);
      result = _injectProperties(result, childRequests, resolveRequest);
    } else {
      result = new constr();
    }

    _postConstruct(constr, result);

    return result;
  }

  exports.resolveInstance = resolveInstance;
});
unwrapExports(instantiation);
var instantiation_1 = instantiation.resolveInstance;

var resolver = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var invokeFactory = function (factoryType, serviceIdentifier, fn) {
    try {
      return fn();
    } catch (error) {
      if (exceptions.isStackOverflowExeption(error)) {
        throw new Error(error_msgs.CIRCULAR_DEPENDENCY_IN_FACTORY(factoryType, serviceIdentifier.toString()));
      } else {
        throw error;
      }
    }
  };

  var _resolveRequest = function (requestScope) {
    return function (request) {
      request.parentContext.setCurrentRequest(request);
      var bindings = request.bindings;
      var childRequests = request.childRequests;
      var targetIsAnArray = request.target && request.target.isArray();
      var targetParentIsNotAnArray = !request.parentRequest || !request.parentRequest.target || !request.target || !request.parentRequest.target.matchesArray(request.target.serviceIdentifier);

      if (targetIsAnArray && targetParentIsNotAnArray) {
        return childRequests.map(function (childRequest) {
          var _f = _resolveRequest(requestScope);

          return _f(childRequest);
        });
      } else {
        var result = null;

        if (request.target.isOptional() && bindings.length === 0) {
          return undefined;
        }

        var binding_1 = bindings[0];
        var isSingleton = binding_1.scope === literal_types.BindingScopeEnum.Singleton;
        var isRequestSingleton = binding_1.scope === literal_types.BindingScopeEnum.Request;

        if (isSingleton && binding_1.activated) {
          return binding_1.cache;
        }

        if (isRequestSingleton && requestScope !== null && requestScope.has(binding_1.id)) {
          return requestScope.get(binding_1.id);
        }

        if (binding_1.type === literal_types.BindingTypeEnum.ConstantValue) {
          result = binding_1.cache;
        } else if (binding_1.type === literal_types.BindingTypeEnum.Function) {
          result = binding_1.cache;
        } else if (binding_1.type === literal_types.BindingTypeEnum.Constructor) {
          result = binding_1.implementationType;
        } else if (binding_1.type === literal_types.BindingTypeEnum.DynamicValue && binding_1.dynamicValue !== null) {
          result = invokeFactory("toDynamicValue", binding_1.serviceIdentifier, function () {
            return binding_1.dynamicValue(request.parentContext);
          });
        } else if (binding_1.type === literal_types.BindingTypeEnum.Factory && binding_1.factory !== null) {
          result = invokeFactory("toFactory", binding_1.serviceIdentifier, function () {
            return binding_1.factory(request.parentContext);
          });
        } else if (binding_1.type === literal_types.BindingTypeEnum.Provider && binding_1.provider !== null) {
          result = invokeFactory("toProvider", binding_1.serviceIdentifier, function () {
            return binding_1.provider(request.parentContext);
          });
        } else if (binding_1.type === literal_types.BindingTypeEnum.Instance && binding_1.implementationType !== null) {
          result = instantiation.resolveInstance(binding_1.implementationType, childRequests, _resolveRequest(requestScope));
        } else {
          var serviceIdentifier = serialization.getServiceIdentifierAsString(request.serviceIdentifier);
          throw new Error(error_msgs.INVALID_BINDING_TYPE + " " + serviceIdentifier);
        }

        if (typeof binding_1.onActivation === "function") {
          result = binding_1.onActivation(request.parentContext, result);
        }

        if (isSingleton) {
          binding_1.cache = result;
          binding_1.activated = true;
        }

        if (isRequestSingleton && requestScope !== null && !requestScope.has(binding_1.id)) {
          requestScope.set(binding_1.id, result);
        }

        return result;
      }
    };
  };

  function resolve(context) {
    var _f = _resolveRequest(context.plan.rootRequest.requestScope);

    return _f(context.plan.rootRequest);
  }

  exports.resolve = resolve;
});
unwrapExports(resolver);
var resolver_1 = resolver.resolve;

var constraint_helpers = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var traverseAncerstors = function (request, constraint) {
    var parent = request.parentRequest;

    if (parent !== null) {
      return constraint(parent) ? true : traverseAncerstors(parent, constraint);
    } else {
      return false;
    }
  };

  exports.traverseAncerstors = traverseAncerstors;

  var taggedConstraint = function (key) {
    return function (value) {
      var constraint = function (request) {
        return request !== null && request.target !== null && request.target.matchesTag(key)(value);
      };

      constraint.metaData = new metadata.Metadata(key, value);
      return constraint;
    };
  };

  exports.taggedConstraint = taggedConstraint;
  var namedConstraint = taggedConstraint(metadata_keys.NAMED_TAG);
  exports.namedConstraint = namedConstraint;

  var typeConstraint = function (type) {
    return function (request) {
      var binding = null;

      if (request !== null) {
        binding = request.bindings[0];

        if (typeof type === "string") {
          var serviceIdentifier = binding.serviceIdentifier;
          return serviceIdentifier === type;
        } else {
          var constructor = request.bindings[0].implementationType;
          return type === constructor;
        }
      }

      return false;
    };
  };

  exports.typeConstraint = typeConstraint;
});
unwrapExports(constraint_helpers);
var constraint_helpers_1 = constraint_helpers.traverseAncerstors;
var constraint_helpers_2 = constraint_helpers.taggedConstraint;
var constraint_helpers_3 = constraint_helpers.namedConstraint;
var constraint_helpers_4 = constraint_helpers.typeConstraint;

var binding_when_syntax = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var BindingWhenSyntax = function () {
    function BindingWhenSyntax(binding) {
      this._binding = binding;
    }

    BindingWhenSyntax.prototype.when = function (constraint) {
      this._binding.constraint = constraint;
      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenTargetNamed = function (name) {
      this._binding.constraint = constraint_helpers.namedConstraint(name);
      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenTargetIsDefault = function () {
      this._binding.constraint = function (request) {
        var targetIsDefault = request.target !== null && !request.target.isNamed() && !request.target.isTagged();
        return targetIsDefault;
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenTargetTagged = function (tag, value) {
      this._binding.constraint = constraint_helpers.taggedConstraint(tag)(value);
      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenInjectedInto = function (parent) {
      this._binding.constraint = function (request) {
        return constraint_helpers.typeConstraint(parent)(request.parentRequest);
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenParentNamed = function (name) {
      this._binding.constraint = function (request) {
        return constraint_helpers.namedConstraint(name)(request.parentRequest);
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenParentTagged = function (tag, value) {
      this._binding.constraint = function (request) {
        return constraint_helpers.taggedConstraint(tag)(value)(request.parentRequest);
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenAnyAncestorIs = function (ancestor) {
      this._binding.constraint = function (request) {
        return constraint_helpers.traverseAncerstors(request, constraint_helpers.typeConstraint(ancestor));
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenNoAncestorIs = function (ancestor) {
      this._binding.constraint = function (request) {
        return !constraint_helpers.traverseAncerstors(request, constraint_helpers.typeConstraint(ancestor));
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenAnyAncestorNamed = function (name) {
      this._binding.constraint = function (request) {
        return constraint_helpers.traverseAncerstors(request, constraint_helpers.namedConstraint(name));
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenNoAncestorNamed = function (name) {
      this._binding.constraint = function (request) {
        return !constraint_helpers.traverseAncerstors(request, constraint_helpers.namedConstraint(name));
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenAnyAncestorTagged = function (tag, value) {
      this._binding.constraint = function (request) {
        return constraint_helpers.traverseAncerstors(request, constraint_helpers.taggedConstraint(tag)(value));
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenNoAncestorTagged = function (tag, value) {
      this._binding.constraint = function (request) {
        return !constraint_helpers.traverseAncerstors(request, constraint_helpers.taggedConstraint(tag)(value));
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenAnyAncestorMatches = function (constraint) {
      this._binding.constraint = function (request) {
        return constraint_helpers.traverseAncerstors(request, constraint);
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    BindingWhenSyntax.prototype.whenNoAncestorMatches = function (constraint) {
      this._binding.constraint = function (request) {
        return !constraint_helpers.traverseAncerstors(request, constraint);
      };

      return new binding_on_syntax.BindingOnSyntax(this._binding);
    };

    return BindingWhenSyntax;
  }();

  exports.BindingWhenSyntax = BindingWhenSyntax;
});
unwrapExports(binding_when_syntax);
var binding_when_syntax_1 = binding_when_syntax.BindingWhenSyntax;

var binding_on_syntax = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var BindingOnSyntax = function () {
    function BindingOnSyntax(binding) {
      this._binding = binding;
    }

    BindingOnSyntax.prototype.onActivation = function (handler) {
      this._binding.onActivation = handler;
      return new binding_when_syntax.BindingWhenSyntax(this._binding);
    };

    return BindingOnSyntax;
  }();

  exports.BindingOnSyntax = BindingOnSyntax;
});
unwrapExports(binding_on_syntax);
var binding_on_syntax_1 = binding_on_syntax.BindingOnSyntax;

var binding_when_on_syntax = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var BindingWhenOnSyntax = function () {
    function BindingWhenOnSyntax(binding) {
      this._binding = binding;
      this._bindingWhenSyntax = new binding_when_syntax.BindingWhenSyntax(this._binding);
      this._bindingOnSyntax = new binding_on_syntax.BindingOnSyntax(this._binding);
    }

    BindingWhenOnSyntax.prototype.when = function (constraint) {
      return this._bindingWhenSyntax.when(constraint);
    };

    BindingWhenOnSyntax.prototype.whenTargetNamed = function (name) {
      return this._bindingWhenSyntax.whenTargetNamed(name);
    };

    BindingWhenOnSyntax.prototype.whenTargetIsDefault = function () {
      return this._bindingWhenSyntax.whenTargetIsDefault();
    };

    BindingWhenOnSyntax.prototype.whenTargetTagged = function (tag, value) {
      return this._bindingWhenSyntax.whenTargetTagged(tag, value);
    };

    BindingWhenOnSyntax.prototype.whenInjectedInto = function (parent) {
      return this._bindingWhenSyntax.whenInjectedInto(parent);
    };

    BindingWhenOnSyntax.prototype.whenParentNamed = function (name) {
      return this._bindingWhenSyntax.whenParentNamed(name);
    };

    BindingWhenOnSyntax.prototype.whenParentTagged = function (tag, value) {
      return this._bindingWhenSyntax.whenParentTagged(tag, value);
    };

    BindingWhenOnSyntax.prototype.whenAnyAncestorIs = function (ancestor) {
      return this._bindingWhenSyntax.whenAnyAncestorIs(ancestor);
    };

    BindingWhenOnSyntax.prototype.whenNoAncestorIs = function (ancestor) {
      return this._bindingWhenSyntax.whenNoAncestorIs(ancestor);
    };

    BindingWhenOnSyntax.prototype.whenAnyAncestorNamed = function (name) {
      return this._bindingWhenSyntax.whenAnyAncestorNamed(name);
    };

    BindingWhenOnSyntax.prototype.whenAnyAncestorTagged = function (tag, value) {
      return this._bindingWhenSyntax.whenAnyAncestorTagged(tag, value);
    };

    BindingWhenOnSyntax.prototype.whenNoAncestorNamed = function (name) {
      return this._bindingWhenSyntax.whenNoAncestorNamed(name);
    };

    BindingWhenOnSyntax.prototype.whenNoAncestorTagged = function (tag, value) {
      return this._bindingWhenSyntax.whenNoAncestorTagged(tag, value);
    };

    BindingWhenOnSyntax.prototype.whenAnyAncestorMatches = function (constraint) {
      return this._bindingWhenSyntax.whenAnyAncestorMatches(constraint);
    };

    BindingWhenOnSyntax.prototype.whenNoAncestorMatches = function (constraint) {
      return this._bindingWhenSyntax.whenNoAncestorMatches(constraint);
    };

    BindingWhenOnSyntax.prototype.onActivation = function (handler) {
      return this._bindingOnSyntax.onActivation(handler);
    };

    return BindingWhenOnSyntax;
  }();

  exports.BindingWhenOnSyntax = BindingWhenOnSyntax;
});
unwrapExports(binding_when_on_syntax);
var binding_when_on_syntax_1 = binding_when_on_syntax.BindingWhenOnSyntax;

var binding_in_syntax = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var BindingInSyntax = function () {
    function BindingInSyntax(binding) {
      this._binding = binding;
    }

    BindingInSyntax.prototype.inRequestScope = function () {
      this._binding.scope = literal_types.BindingScopeEnum.Request;
      return new binding_when_on_syntax.BindingWhenOnSyntax(this._binding);
    };

    BindingInSyntax.prototype.inSingletonScope = function () {
      this._binding.scope = literal_types.BindingScopeEnum.Singleton;
      return new binding_when_on_syntax.BindingWhenOnSyntax(this._binding);
    };

    BindingInSyntax.prototype.inTransientScope = function () {
      this._binding.scope = literal_types.BindingScopeEnum.Transient;
      return new binding_when_on_syntax.BindingWhenOnSyntax(this._binding);
    };

    return BindingInSyntax;
  }();

  exports.BindingInSyntax = BindingInSyntax;
});
unwrapExports(binding_in_syntax);
var binding_in_syntax_1 = binding_in_syntax.BindingInSyntax;

var binding_in_when_on_syntax = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var BindingInWhenOnSyntax = function () {
    function BindingInWhenOnSyntax(binding) {
      this._binding = binding;
      this._bindingWhenSyntax = new binding_when_syntax.BindingWhenSyntax(this._binding);
      this._bindingOnSyntax = new binding_on_syntax.BindingOnSyntax(this._binding);
      this._bindingInSyntax = new binding_in_syntax.BindingInSyntax(binding);
    }

    BindingInWhenOnSyntax.prototype.inRequestScope = function () {
      return this._bindingInSyntax.inRequestScope();
    };

    BindingInWhenOnSyntax.prototype.inSingletonScope = function () {
      return this._bindingInSyntax.inSingletonScope();
    };

    BindingInWhenOnSyntax.prototype.inTransientScope = function () {
      return this._bindingInSyntax.inTransientScope();
    };

    BindingInWhenOnSyntax.prototype.when = function (constraint) {
      return this._bindingWhenSyntax.when(constraint);
    };

    BindingInWhenOnSyntax.prototype.whenTargetNamed = function (name) {
      return this._bindingWhenSyntax.whenTargetNamed(name);
    };

    BindingInWhenOnSyntax.prototype.whenTargetIsDefault = function () {
      return this._bindingWhenSyntax.whenTargetIsDefault();
    };

    BindingInWhenOnSyntax.prototype.whenTargetTagged = function (tag, value) {
      return this._bindingWhenSyntax.whenTargetTagged(tag, value);
    };

    BindingInWhenOnSyntax.prototype.whenInjectedInto = function (parent) {
      return this._bindingWhenSyntax.whenInjectedInto(parent);
    };

    BindingInWhenOnSyntax.prototype.whenParentNamed = function (name) {
      return this._bindingWhenSyntax.whenParentNamed(name);
    };

    BindingInWhenOnSyntax.prototype.whenParentTagged = function (tag, value) {
      return this._bindingWhenSyntax.whenParentTagged(tag, value);
    };

    BindingInWhenOnSyntax.prototype.whenAnyAncestorIs = function (ancestor) {
      return this._bindingWhenSyntax.whenAnyAncestorIs(ancestor);
    };

    BindingInWhenOnSyntax.prototype.whenNoAncestorIs = function (ancestor) {
      return this._bindingWhenSyntax.whenNoAncestorIs(ancestor);
    };

    BindingInWhenOnSyntax.prototype.whenAnyAncestorNamed = function (name) {
      return this._bindingWhenSyntax.whenAnyAncestorNamed(name);
    };

    BindingInWhenOnSyntax.prototype.whenAnyAncestorTagged = function (tag, value) {
      return this._bindingWhenSyntax.whenAnyAncestorTagged(tag, value);
    };

    BindingInWhenOnSyntax.prototype.whenNoAncestorNamed = function (name) {
      return this._bindingWhenSyntax.whenNoAncestorNamed(name);
    };

    BindingInWhenOnSyntax.prototype.whenNoAncestorTagged = function (tag, value) {
      return this._bindingWhenSyntax.whenNoAncestorTagged(tag, value);
    };

    BindingInWhenOnSyntax.prototype.whenAnyAncestorMatches = function (constraint) {
      return this._bindingWhenSyntax.whenAnyAncestorMatches(constraint);
    };

    BindingInWhenOnSyntax.prototype.whenNoAncestorMatches = function (constraint) {
      return this._bindingWhenSyntax.whenNoAncestorMatches(constraint);
    };

    BindingInWhenOnSyntax.prototype.onActivation = function (handler) {
      return this._bindingOnSyntax.onActivation(handler);
    };

    return BindingInWhenOnSyntax;
  }();

  exports.BindingInWhenOnSyntax = BindingInWhenOnSyntax;
});
unwrapExports(binding_in_when_on_syntax);
var binding_in_when_on_syntax_1 = binding_in_when_on_syntax.BindingInWhenOnSyntax;

var binding_to_syntax = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var BindingToSyntax = function () {
    function BindingToSyntax(binding) {
      this._binding = binding;
    }

    BindingToSyntax.prototype.to = function (constructor) {
      this._binding.type = literal_types.BindingTypeEnum.Instance;
      this._binding.implementationType = constructor;
      return new binding_in_when_on_syntax.BindingInWhenOnSyntax(this._binding);
    };

    BindingToSyntax.prototype.toSelf = function () {
      if (typeof this._binding.serviceIdentifier !== "function") {
        throw new Error("" + error_msgs.INVALID_TO_SELF_VALUE);
      }

      var self = this._binding.serviceIdentifier;
      return this.to(self);
    };

    BindingToSyntax.prototype.toConstantValue = function (value) {
      this._binding.type = literal_types.BindingTypeEnum.ConstantValue;
      this._binding.cache = value;
      this._binding.dynamicValue = null;
      this._binding.implementationType = null;
      return new binding_when_on_syntax.BindingWhenOnSyntax(this._binding);
    };

    BindingToSyntax.prototype.toDynamicValue = function (func) {
      this._binding.type = literal_types.BindingTypeEnum.DynamicValue;
      this._binding.cache = null;
      this._binding.dynamicValue = func;
      this._binding.implementationType = null;
      return new binding_in_when_on_syntax.BindingInWhenOnSyntax(this._binding);
    };

    BindingToSyntax.prototype.toConstructor = function (constructor) {
      this._binding.type = literal_types.BindingTypeEnum.Constructor;
      this._binding.implementationType = constructor;
      return new binding_when_on_syntax.BindingWhenOnSyntax(this._binding);
    };

    BindingToSyntax.prototype.toFactory = function (factory) {
      this._binding.type = literal_types.BindingTypeEnum.Factory;
      this._binding.factory = factory;
      return new binding_when_on_syntax.BindingWhenOnSyntax(this._binding);
    };

    BindingToSyntax.prototype.toFunction = function (func) {
      if (typeof func !== "function") {
        throw new Error(error_msgs.INVALID_FUNCTION_BINDING);
      }

      var bindingWhenOnSyntax = this.toConstantValue(func);
      this._binding.type = literal_types.BindingTypeEnum.Function;
      return bindingWhenOnSyntax;
    };

    BindingToSyntax.prototype.toAutoFactory = function (serviceIdentifier) {
      this._binding.type = literal_types.BindingTypeEnum.Factory;

      this._binding.factory = function (context) {
        var autofactory = function () {
          return context.container.get(serviceIdentifier);
        };

        return autofactory;
      };

      return new binding_when_on_syntax.BindingWhenOnSyntax(this._binding);
    };

    BindingToSyntax.prototype.toProvider = function (provider) {
      this._binding.type = literal_types.BindingTypeEnum.Provider;
      this._binding.provider = provider;
      return new binding_when_on_syntax.BindingWhenOnSyntax(this._binding);
    };

    BindingToSyntax.prototype.toService = function (service) {
      this.toDynamicValue(function (context) {
        return context.container.get(service);
      });
    };

    return BindingToSyntax;
  }();

  exports.BindingToSyntax = BindingToSyntax;
});
unwrapExports(binding_to_syntax);
var binding_to_syntax_1 = binding_to_syntax.BindingToSyntax;

var container_snapshot = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var ContainerSnapshot = function () {
    function ContainerSnapshot() {}

    ContainerSnapshot.of = function (bindings, middleware) {
      var snapshot = new ContainerSnapshot();
      snapshot.bindings = bindings;
      snapshot.middleware = middleware;
      return snapshot;
    };

    return ContainerSnapshot;
  }();

  exports.ContainerSnapshot = ContainerSnapshot;
});
unwrapExports(container_snapshot);
var container_snapshot_1 = container_snapshot.ContainerSnapshot;

var lookup = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var Lookup = function () {
    function Lookup() {
      this._map = new Map();
    }

    Lookup.prototype.getMap = function () {
      return this._map;
    };

    Lookup.prototype.add = function (serviceIdentifier, value) {
      if (serviceIdentifier === null || serviceIdentifier === undefined) {
        throw new Error(error_msgs.NULL_ARGUMENT);
      }

      if (value === null || value === undefined) {
        throw new Error(error_msgs.NULL_ARGUMENT);
      }

      var entry = this._map.get(serviceIdentifier);

      if (entry !== undefined) {
        entry.push(value);

        this._map.set(serviceIdentifier, entry);
      } else {
        this._map.set(serviceIdentifier, [value]);
      }
    };

    Lookup.prototype.get = function (serviceIdentifier) {
      if (serviceIdentifier === null || serviceIdentifier === undefined) {
        throw new Error(error_msgs.NULL_ARGUMENT);
      }

      var entry = this._map.get(serviceIdentifier);

      if (entry !== undefined) {
        return entry;
      } else {
        throw new Error(error_msgs.KEY_NOT_FOUND);
      }
    };

    Lookup.prototype.remove = function (serviceIdentifier) {
      if (serviceIdentifier === null || serviceIdentifier === undefined) {
        throw new Error(error_msgs.NULL_ARGUMENT);
      }

      if (!this._map.delete(serviceIdentifier)) {
        throw new Error(error_msgs.KEY_NOT_FOUND);
      }
    };

    Lookup.prototype.removeByCondition = function (condition) {
      var _this = this;

      this._map.forEach(function (entries, key) {
        var updatedEntries = entries.filter(function (entry) {
          return !condition(entry);
        });

        if (updatedEntries.length > 0) {
          _this._map.set(key, updatedEntries);
        } else {
          _this._map.delete(key);
        }
      });
    };

    Lookup.prototype.hasKey = function (serviceIdentifier) {
      if (serviceIdentifier === null || serviceIdentifier === undefined) {
        throw new Error(error_msgs.NULL_ARGUMENT);
      }

      return this._map.has(serviceIdentifier);
    };

    Lookup.prototype.clone = function () {
      var copy = new Lookup();

      this._map.forEach(function (value, key) {
        value.forEach(function (b) {
          return copy.add(key, b.clone());
        });
      });

      return copy;
    };

    Lookup.prototype.traverse = function (func) {
      this._map.forEach(function (value, key) {
        func(key, value);
      });
    };

    return Lookup;
  }();

  exports.Lookup = Lookup;
});
unwrapExports(lookup);
var lookup_1 = lookup.Lookup;

var container = createCommonjsModule(function (module, exports) {

  var __awaiter = commonjsGlobal && commonjsGlobal.__awaiter || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
      function fulfilled(value) {
        try {
          step(generator.next(value));
        } catch (e) {
          reject(e);
        }
      }

      function rejected(value) {
        try {
          step(generator["throw"](value));
        } catch (e) {
          reject(e);
        }
      }

      function step(result) {
        result.done ? resolve(result.value) : new P(function (resolve) {
          resolve(result.value);
        }).then(fulfilled, rejected);
      }

      step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
  };

  var __generator = commonjsGlobal && commonjsGlobal.__generator || function (thisArg, body) {
    var _ = {
      label: 0,
      sent: function () {
        if (t[0] & 1) throw t[1];
        return t[1];
      },
      trys: [],
      ops: []
    },
        f,
        y,
        t,
        g;
    return g = {
      next: verb(0),
      "throw": verb(1),
      "return": verb(2)
    }, typeof Symbol === "function" && (g[Symbol.iterator] = function () {
      return this;
    }), g;

    function verb(n) {
      return function (v) {
        return step([n, v]);
      };
    }

    function step(op) {
      if (f) throw new TypeError("Generator is already executing.");

      while (_) try {
        if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
        if (y = 0, t) op = [0, t.value];

        switch (op[0]) {
          case 0:
          case 1:
            t = op;
            break;

          case 4:
            _.label++;
            return {
              value: op[1],
              done: false
            };

          case 5:
            _.label++;
            y = op[1];
            op = [0];
            continue;

          case 7:
            op = _.ops.pop();

            _.trys.pop();

            continue;

          default:
            if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) {
              _ = 0;
              continue;
            }

            if (op[0] === 3 && (!t || op[1] > t[0] && op[1] < t[3])) {
              _.label = op[1];
              break;
            }

            if (op[0] === 6 && _.label < t[1]) {
              _.label = t[1];
              t = op;
              break;
            }

            if (t && _.label < t[2]) {
              _.label = t[2];

              _.ops.push(op);

              break;
            }

            if (t[2]) _.ops.pop();

            _.trys.pop();

            continue;
        }

        op = body.call(thisArg, _);
      } catch (e) {
        op = [6, e];
        y = 0;
      } finally {
        f = t = 0;
      }

      if (op[0] & 5) throw op[1];
      return {
        value: op[0] ? op[1] : void 0,
        done: true
      };
    }
  };

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var Container = function () {
    function Container(containerOptions) {
      var options = containerOptions || {};

      if (typeof options !== "object") {
        throw new Error("" + error_msgs.CONTAINER_OPTIONS_MUST_BE_AN_OBJECT);
      }

      if (options.defaultScope === undefined) {
        options.defaultScope = literal_types.BindingScopeEnum.Transient;
      } else if (options.defaultScope !== literal_types.BindingScopeEnum.Singleton && options.defaultScope !== literal_types.BindingScopeEnum.Transient && options.defaultScope !== literal_types.BindingScopeEnum.Request) {
        throw new Error("" + error_msgs.CONTAINER_OPTIONS_INVALID_DEFAULT_SCOPE);
      }

      if (options.autoBindInjectable === undefined) {
        options.autoBindInjectable = false;
      } else if (typeof options.autoBindInjectable !== "boolean") {
        throw new Error("" + error_msgs.CONTAINER_OPTIONS_INVALID_AUTO_BIND_INJECTABLE);
      }

      if (options.skipBaseClassChecks === undefined) {
        options.skipBaseClassChecks = false;
      } else if (typeof options.skipBaseClassChecks !== "boolean") {
        throw new Error("" + error_msgs.CONTAINER_OPTIONS_INVALID_SKIP_BASE_CHECK);
      }

      this.options = {
        autoBindInjectable: options.autoBindInjectable,
        defaultScope: options.defaultScope,
        skipBaseClassChecks: options.skipBaseClassChecks
      };
      this.id = id_1.id();
      this._bindingDictionary = new lookup.Lookup();
      this._snapshots = [];
      this._middleware = null;
      this.parent = null;
      this._metadataReader = new metadata_reader.MetadataReader();
    }

    Container.merge = function (container1, container2) {
      var container = new Container();
      var bindingDictionary = planner.getBindingDictionary(container);
      var bindingDictionary1 = planner.getBindingDictionary(container1);
      var bindingDictionary2 = planner.getBindingDictionary(container2);

      function copyDictionary(origin, destination) {
        origin.traverse(function (key, value) {
          value.forEach(function (binding) {
            destination.add(binding.serviceIdentifier, binding.clone());
          });
        });
      }

      copyDictionary(bindingDictionary1, bindingDictionary);
      copyDictionary(bindingDictionary2, bindingDictionary);
      return container;
    };

    Container.prototype.load = function () {
      var modules = [];

      for (var _i = 0; _i < arguments.length; _i++) {
        modules[_i] = arguments[_i];
      }

      var getHelpers = this._getContainerModuleHelpersFactory();

      for (var _a = 0, modules_1 = modules; _a < modules_1.length; _a++) {
        var currentModule = modules_1[_a];
        var containerModuleHelpers = getHelpers(currentModule.id);
        currentModule.registry(containerModuleHelpers.bindFunction, containerModuleHelpers.unbindFunction, containerModuleHelpers.isboundFunction, containerModuleHelpers.rebindFunction);
      }
    };

    Container.prototype.loadAsync = function () {
      var modules = [];

      for (var _i = 0; _i < arguments.length; _i++) {
        modules[_i] = arguments[_i];
      }

      return __awaiter(this, void 0, void 0, function () {
        var getHelpers, _a, modules_2, currentModule, containerModuleHelpers;

        return __generator(this, function (_b) {
          switch (_b.label) {
            case 0:
              getHelpers = this._getContainerModuleHelpersFactory();
              _a = 0, modules_2 = modules;
              _b.label = 1;

            case 1:
              if (!(_a < modules_2.length)) return [3, 4];
              currentModule = modules_2[_a];
              containerModuleHelpers = getHelpers(currentModule.id);
              return [4, currentModule.registry(containerModuleHelpers.bindFunction, containerModuleHelpers.unbindFunction, containerModuleHelpers.isboundFunction, containerModuleHelpers.rebindFunction)];

            case 2:
              _b.sent();

              _b.label = 3;

            case 3:
              _a++;
              return [3, 1];

            case 4:
              return [2];
          }
        });
      });
    };

    Container.prototype.unload = function () {
      var _this = this;

      var modules = [];

      for (var _i = 0; _i < arguments.length; _i++) {
        modules[_i] = arguments[_i];
      }

      var conditionFactory = function (expected) {
        return function (item) {
          return item.moduleId === expected;
        };
      };

      modules.forEach(function (module) {
        var condition = conditionFactory(module.id);

        _this._bindingDictionary.removeByCondition(condition);
      });
    };

    Container.prototype.bind = function (serviceIdentifier) {
      var scope = this.options.defaultScope || literal_types.BindingScopeEnum.Transient;
      var binding$1 = new binding.Binding(serviceIdentifier, scope);

      this._bindingDictionary.add(serviceIdentifier, binding$1);

      return new binding_to_syntax.BindingToSyntax(binding$1);
    };

    Container.prototype.rebind = function (serviceIdentifier) {
      this.unbind(serviceIdentifier);
      return this.bind(serviceIdentifier);
    };

    Container.prototype.unbind = function (serviceIdentifier) {
      try {
        this._bindingDictionary.remove(serviceIdentifier);
      } catch (e) {
        throw new Error(error_msgs.CANNOT_UNBIND + " " + serialization.getServiceIdentifierAsString(serviceIdentifier));
      }
    };

    Container.prototype.unbindAll = function () {
      this._bindingDictionary = new lookup.Lookup();
    };

    Container.prototype.isBound = function (serviceIdentifier) {
      var bound = this._bindingDictionary.hasKey(serviceIdentifier);

      if (!bound && this.parent) {
        bound = this.parent.isBound(serviceIdentifier);
      }

      return bound;
    };

    Container.prototype.isBoundNamed = function (serviceIdentifier, named) {
      return this.isBoundTagged(serviceIdentifier, metadata_keys.NAMED_TAG, named);
    };

    Container.prototype.isBoundTagged = function (serviceIdentifier, key, value) {
      var bound = false;

      if (this._bindingDictionary.hasKey(serviceIdentifier)) {
        var bindings = this._bindingDictionary.get(serviceIdentifier);

        var request_1 = planner.createMockRequest(this, serviceIdentifier, key, value);
        bound = bindings.some(function (b) {
          return b.constraint(request_1);
        });
      }

      if (!bound && this.parent) {
        bound = this.parent.isBoundTagged(serviceIdentifier, key, value);
      }

      return bound;
    };

    Container.prototype.snapshot = function () {
      this._snapshots.push(container_snapshot.ContainerSnapshot.of(this._bindingDictionary.clone(), this._middleware));
    };

    Container.prototype.restore = function () {
      var snapshot = this._snapshots.pop();

      if (snapshot === undefined) {
        throw new Error(error_msgs.NO_MORE_SNAPSHOTS_AVAILABLE);
      }

      this._bindingDictionary = snapshot.bindings;
      this._middleware = snapshot.middleware;
    };

    Container.prototype.createChild = function (containerOptions) {
      var child = new Container(containerOptions || this.options);
      child.parent = this;
      return child;
    };

    Container.prototype.applyMiddleware = function () {
      var middlewares = [];

      for (var _i = 0; _i < arguments.length; _i++) {
        middlewares[_i] = arguments[_i];
      }

      var initial = this._middleware ? this._middleware : this._planAndResolve();
      this._middleware = middlewares.reduce(function (prev, curr) {
        return curr(prev);
      }, initial);
    };

    Container.prototype.applyCustomMetadataReader = function (metadataReader) {
      this._metadataReader = metadataReader;
    };

    Container.prototype.get = function (serviceIdentifier) {
      return this._get(false, false, literal_types.TargetTypeEnum.Variable, serviceIdentifier);
    };

    Container.prototype.getTagged = function (serviceIdentifier, key, value) {
      return this._get(false, false, literal_types.TargetTypeEnum.Variable, serviceIdentifier, key, value);
    };

    Container.prototype.getNamed = function (serviceIdentifier, named) {
      return this.getTagged(serviceIdentifier, metadata_keys.NAMED_TAG, named);
    };

    Container.prototype.getAll = function (serviceIdentifier) {
      return this._get(true, true, literal_types.TargetTypeEnum.Variable, serviceIdentifier);
    };

    Container.prototype.getAllTagged = function (serviceIdentifier, key, value) {
      return this._get(false, true, literal_types.TargetTypeEnum.Variable, serviceIdentifier, key, value);
    };

    Container.prototype.getAllNamed = function (serviceIdentifier, named) {
      return this.getAllTagged(serviceIdentifier, metadata_keys.NAMED_TAG, named);
    };

    Container.prototype.resolve = function (constructorFunction) {
      var tempContainer = this.createChild();
      tempContainer.bind(constructorFunction).toSelf();
      return tempContainer.get(constructorFunction);
    };

    Container.prototype._getContainerModuleHelpersFactory = function () {
      var _this = this;

      var setModuleId = function (bindingToSyntax, moduleId) {
        bindingToSyntax._binding.moduleId = moduleId;
      };

      var getBindFunction = function (moduleId) {
        return function (serviceIdentifier) {
          var _bind = _this.bind.bind(_this);

          var bindingToSyntax = _bind(serviceIdentifier);

          setModuleId(bindingToSyntax, moduleId);
          return bindingToSyntax;
        };
      };

      var getUnbindFunction = function (moduleId) {
        return function (serviceIdentifier) {
          var _unbind = _this.unbind.bind(_this);

          _unbind(serviceIdentifier);
        };
      };

      var getIsboundFunction = function (moduleId) {
        return function (serviceIdentifier) {
          var _isBound = _this.isBound.bind(_this);

          return _isBound(serviceIdentifier);
        };
      };

      var getRebindFunction = function (moduleId) {
        return function (serviceIdentifier) {
          var _rebind = _this.rebind.bind(_this);

          var bindingToSyntax = _rebind(serviceIdentifier);

          setModuleId(bindingToSyntax, moduleId);
          return bindingToSyntax;
        };
      };

      return function (mId) {
        return {
          bindFunction: getBindFunction(mId),
          isboundFunction: getIsboundFunction(),
          rebindFunction: getRebindFunction(mId),
          unbindFunction: getUnbindFunction()
        };
      };
    };

    Container.prototype._get = function (avoidConstraints, isMultiInject, targetType, serviceIdentifier, key, value) {
      var result = null;
      var defaultArgs = {
        avoidConstraints: avoidConstraints,
        contextInterceptor: function (context) {
          return context;
        },
        isMultiInject: isMultiInject,
        key: key,
        serviceIdentifier: serviceIdentifier,
        targetType: targetType,
        value: value
      };

      if (this._middleware) {
        result = this._middleware(defaultArgs);

        if (result === undefined || result === null) {
          throw new Error(error_msgs.INVALID_MIDDLEWARE_RETURN);
        }
      } else {
        result = this._planAndResolve()(defaultArgs);
      }

      return result;
    };

    Container.prototype._planAndResolve = function () {
      var _this = this;

      return function (args) {
        var context = planner.plan(_this._metadataReader, _this, args.isMultiInject, args.targetType, args.serviceIdentifier, args.key, args.value, args.avoidConstraints);
        context = args.contextInterceptor(context);
        var result = resolver.resolve(context);
        return result;
      };
    };

    return Container;
  }();

  exports.Container = Container;
});
unwrapExports(container);
var container_1 = container.Container;

var container_module = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  var ContainerModule = function () {
    function ContainerModule(registry) {
      this.id = id_1.id();
      this.registry = registry;
    }

    return ContainerModule;
  }();

  exports.ContainerModule = ContainerModule;

  var AsyncContainerModule = function () {
    function AsyncContainerModule(registry) {
      this.id = id_1.id();
      this.registry = registry;
    }

    return AsyncContainerModule;
  }();

  exports.AsyncContainerModule = AsyncContainerModule;
});
unwrapExports(container_module);
var container_module_1 = container_module.ContainerModule;
var container_module_2 = container_module.AsyncContainerModule;

var injectable_1 = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function injectable() {
    return function (target) {
      if (Reflect.hasOwnMetadata(metadata_keys.PARAM_TYPES, target)) {
        throw new Error(error_msgs.DUPLICATED_INJECTABLE_DECORATOR);
      }

      var types = Reflect.getMetadata(metadata_keys.DESIGN_PARAM_TYPES, target) || [];
      Reflect.defineMetadata(metadata_keys.PARAM_TYPES, types, target);
      return target;
    };
  }

  exports.injectable = injectable;
});
unwrapExports(injectable_1);
var injectable_2 = injectable_1.injectable;

var tagged_1 = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function tagged(metadataKey, metadataValue) {
    return function (target, targetKey, index) {
      var metadata$1 = new metadata.Metadata(metadataKey, metadataValue);

      if (typeof index === "number") {
        decorator_utils.tagParameter(target, targetKey, index, metadata$1);
      } else {
        decorator_utils.tagProperty(target, targetKey, metadata$1);
      }
    };
  }

  exports.tagged = tagged;
});
unwrapExports(tagged_1);
var tagged_2 = tagged_1.tagged;

var named_1 = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function named(name) {
    return function (target, targetKey, index) {
      var metadata$1 = new metadata.Metadata(metadata_keys.NAMED_TAG, name);

      if (typeof index === "number") {
        decorator_utils.tagParameter(target, targetKey, index, metadata$1);
      } else {
        decorator_utils.tagProperty(target, targetKey, metadata$1);
      }
    };
  }

  exports.named = named;
});
unwrapExports(named_1);
var named_2 = named_1.named;

var optional_1 = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function optional() {
    return function (target, targetKey, index) {
      var metadata$1 = new metadata.Metadata(metadata_keys.OPTIONAL_TAG, true);

      if (typeof index === "number") {
        decorator_utils.tagParameter(target, targetKey, index, metadata$1);
      } else {
        decorator_utils.tagProperty(target, targetKey, metadata$1);
      }
    };
  }

  exports.optional = optional;
});
unwrapExports(optional_1);
var optional_2 = optional_1.optional;

var unmanaged_1 = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function unmanaged() {
    return function (target, targetKey, index) {
      var metadata$1 = new metadata.Metadata(metadata_keys.UNMANAGED_TAG, true);
      decorator_utils.tagParameter(target, targetKey, index, metadata$1);
    };
  }

  exports.unmanaged = unmanaged;
});
unwrapExports(unmanaged_1);
var unmanaged_2 = unmanaged_1.unmanaged;

var multi_inject = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function multiInject(serviceIdentifier) {
    return function (target, targetKey, index) {
      var metadata$1 = new metadata.Metadata(metadata_keys.MULTI_INJECT_TAG, serviceIdentifier);

      if (typeof index === "number") {
        decorator_utils.tagParameter(target, targetKey, index, metadata$1);
      } else {
        decorator_utils.tagProperty(target, targetKey, metadata$1);
      }
    };
  }

  exports.multiInject = multiInject;
});
unwrapExports(multi_inject);
var multi_inject_1 = multi_inject.multiInject;

var target_name = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function targetName(name) {
    return function (target, targetKey, index) {
      var metadata$1 = new metadata.Metadata(metadata_keys.NAME_TAG, name);
      decorator_utils.tagParameter(target, targetKey, index, metadata$1);
    };
  }

  exports.targetName = targetName;
});
unwrapExports(target_name);
var target_name_1 = target_name.targetName;

var post_construct = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function postConstruct() {
    return function (target, propertyKey, descriptor) {
      var metadata$1 = new metadata.Metadata(metadata_keys.POST_CONSTRUCT, propertyKey);

      if (Reflect.hasOwnMetadata(metadata_keys.POST_CONSTRUCT, target.constructor)) {
        throw new Error(error_msgs.MULTIPLE_POST_CONSTRUCT_METHODS);
      }

      Reflect.defineMetadata(metadata_keys.POST_CONSTRUCT, metadata$1, target.constructor);
    };
  }

  exports.postConstruct = postConstruct;
});
unwrapExports(post_construct);
var post_construct_1 = post_construct.postConstruct;

var binding_utils = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  exports.multiBindToService = function (container) {
    return function (service) {
      return function () {
        var types = [];

        for (var _i = 0; _i < arguments.length; _i++) {
          types[_i] = arguments[_i];
        }

        return types.forEach(function (t) {
          return container.bind(t).toService(service);
        });
      };
    };
  };
});
unwrapExports(binding_utils);
var binding_utils_1 = binding_utils.multiBindToService;

var inversify = createCommonjsModule(function (module, exports) {

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.METADATA_KEY = metadata_keys;
  exports.Container = container.Container;
  exports.BindingScopeEnum = literal_types.BindingScopeEnum;
  exports.BindingTypeEnum = literal_types.BindingTypeEnum;
  exports.TargetTypeEnum = literal_types.TargetTypeEnum;
  exports.AsyncContainerModule = container_module.AsyncContainerModule;
  exports.ContainerModule = container_module.ContainerModule;
  exports.injectable = injectable_1.injectable;
  exports.tagged = tagged_1.tagged;
  exports.named = named_1.named;
  exports.inject = inject_1.inject;
  exports.LazyServiceIdentifer = inject_1.LazyServiceIdentifer;
  exports.optional = optional_1.optional;
  exports.unmanaged = unmanaged_1.unmanaged;
  exports.multiInject = multi_inject.multiInject;
  exports.targetName = target_name.targetName;
  exports.postConstruct = post_construct.postConstruct;
  exports.MetadataReader = metadata_reader.MetadataReader;
  exports.id = id_1.id;
  exports.decorate = decorator_utils.decorate;
  exports.traverseAncerstors = constraint_helpers.traverseAncerstors;
  exports.taggedConstraint = constraint_helpers.taggedConstraint;
  exports.namedConstraint = constraint_helpers.namedConstraint;
  exports.typeConstraint = constraint_helpers.typeConstraint;
  exports.getServiceIdentifierAsString = serialization.getServiceIdentifierAsString;
  exports.multiBindToService = binding_utils.multiBindToService;
});
unwrapExports(inversify);
var inversify_1 = inversify.METADATA_KEY;
var inversify_2 = inversify.Container;
var inversify_3 = inversify.BindingScopeEnum;
var inversify_4 = inversify.BindingTypeEnum;
var inversify_5 = inversify.TargetTypeEnum;
var inversify_6 = inversify.AsyncContainerModule;
var inversify_7 = inversify.ContainerModule;
var inversify_8 = inversify.injectable;
var inversify_9 = inversify.tagged;
var inversify_10 = inversify.named;
var inversify_11 = inversify.inject;
var inversify_12 = inversify.LazyServiceIdentifer;
var inversify_13 = inversify.optional;
var inversify_14 = inversify.unmanaged;
var inversify_15 = inversify.multiInject;
var inversify_16 = inversify.targetName;
var inversify_17 = inversify.postConstruct;
var inversify_18 = inversify.MetadataReader;
var inversify_19 = inversify.id;
var inversify_20 = inversify.decorate;
var inversify_21 = inversify.traverseAncerstors;
var inversify_22 = inversify.taggedConstraint;
var inversify_23 = inversify.namedConstraint;
var inversify_24 = inversify.typeConstraint;
var inversify_25 = inversify.getServiceIdentifierAsString;
var inversify_26 = inversify.multiBindToService;

var slice = [].slice;
var factories = {};

var construct = function (C, argsLength, args) {
  if (!(argsLength in factories)) {
    for (var list = [], i = 0; i < argsLength; i++) list[i] = 'a[' + i + ']'; // eslint-disable-next-line no-new-func


    factories[argsLength] = Function('C,a', 'return new C(' + list.join(',') + ')');
  }

  return factories[argsLength](C, args);
}; // `Function.prototype.bind` method implementation
// https://tc39.github.io/ecma262/#sec-function.prototype.bind


var functionBind = Function.bind || function bind(that
/* , ...args */
) {
  var fn = aFunction(this);
  var partArgs = slice.call(arguments, 1);

  var boundFunction = function bound()
  /* args... */
  {
    var args = partArgs.concat(slice.call(arguments));
    return this instanceof boundFunction ? construct(fn, args.length, args) : fn.apply(that, args);
  };

  if (isObject(fn.prototype)) boundFunction.prototype = fn.prototype;
  return boundFunction;
};

// https://tc39.github.io/ecma262/#sec-function.prototype.bind

_export({
  target: 'Function',
  proto: true
}, {
  bind: functionBind
});

var entryVirtual = function (CONSTRUCTOR) {
  return path[CONSTRUCTOR + 'Prototype'];
};

var bind = entryVirtual('Function').bind;

var FunctionPrototype = Function.prototype;

var bind_1 = function (it) {
  var own = it.bind;
  return it === FunctionPrototype || it instanceof Function && own === FunctionPrototype.bind ? bind : own;
};

var bind$1 = bind_1;

var bind$2 = bind$1;

var runtime_1 = createCommonjsModule(function (module) {
  /**
   * Copyright (c) 2014-present, Facebook, Inc.
   *
   * This source code is licensed under the MIT license found in the
   * LICENSE file in the root directory of this source tree.
   */
  var runtime = function (exports) {

    var Op = Object.prototype;
    var hasOwn = Op.hasOwnProperty;
    var undefined$1; // More compressible than void 0.

    var $Symbol = typeof Symbol === "function" ? Symbol : {};
    var iteratorSymbol = $Symbol.iterator || "@@iterator";
    var asyncIteratorSymbol = $Symbol.asyncIterator || "@@asyncIterator";
    var toStringTagSymbol = $Symbol.toStringTag || "@@toStringTag";

    function wrap(innerFn, outerFn, self, tryLocsList) {
      // If outerFn provided and outerFn.prototype is a Generator, then outerFn.prototype instanceof Generator.
      var protoGenerator = outerFn && outerFn.prototype instanceof Generator ? outerFn : Generator;
      var generator = Object.create(protoGenerator.prototype);
      var context = new Context(tryLocsList || []); // The ._invoke method unifies the implementations of the .next,
      // .throw, and .return methods.

      generator._invoke = makeInvokeMethod(innerFn, self, context);
      return generator;
    }

    exports.wrap = wrap; // Try/catch helper to minimize deoptimizations. Returns a completion
    // record like context.tryEntries[i].completion. This interface could
    // have been (and was previously) designed to take a closure to be
    // invoked without arguments, but in all the cases we care about we
    // already have an existing method we want to call, so there's no need
    // to create a new function object. We can even get away with assuming
    // the method takes exactly one argument, since that happens to be true
    // in every case, so we don't have to touch the arguments object. The
    // only additional allocation required is the completion record, which
    // has a stable shape and so hopefully should be cheap to allocate.

    function tryCatch(fn, obj, arg) {
      try {
        return {
          type: "normal",
          arg: fn.call(obj, arg)
        };
      } catch (err) {
        return {
          type: "throw",
          arg: err
        };
      }
    }

    var GenStateSuspendedStart = "suspendedStart";
    var GenStateSuspendedYield = "suspendedYield";
    var GenStateExecuting = "executing";
    var GenStateCompleted = "completed"; // Returning this object from the innerFn has the same effect as
    // breaking out of the dispatch switch statement.

    var ContinueSentinel = {}; // Dummy constructor functions that we use as the .constructor and
    // .constructor.prototype properties for functions that return Generator
    // objects. For full spec compliance, you may wish to configure your
    // minifier not to mangle the names of these two functions.

    function Generator() {}

    function GeneratorFunction() {}

    function GeneratorFunctionPrototype() {} // This is a polyfill for %IteratorPrototype% for environments that
    // don't natively support it.


    var IteratorPrototype = {};

    IteratorPrototype[iteratorSymbol] = function () {
      return this;
    };

    var getProto = Object.getPrototypeOf;
    var NativeIteratorPrototype = getProto && getProto(getProto(values([])));

    if (NativeIteratorPrototype && NativeIteratorPrototype !== Op && hasOwn.call(NativeIteratorPrototype, iteratorSymbol)) {
      // This environment has a native %IteratorPrototype%; use it instead
      // of the polyfill.
      IteratorPrototype = NativeIteratorPrototype;
    }

    var Gp = GeneratorFunctionPrototype.prototype = Generator.prototype = Object.create(IteratorPrototype);
    GeneratorFunction.prototype = Gp.constructor = GeneratorFunctionPrototype;
    GeneratorFunctionPrototype.constructor = GeneratorFunction;
    GeneratorFunctionPrototype[toStringTagSymbol] = GeneratorFunction.displayName = "GeneratorFunction"; // Helper for defining the .next, .throw, and .return methods of the
    // Iterator interface in terms of a single ._invoke method.

    function defineIteratorMethods(prototype) {
      ["next", "throw", "return"].forEach(function (method) {
        prototype[method] = function (arg) {
          return this._invoke(method, arg);
        };
      });
    }

    exports.isGeneratorFunction = function (genFun) {
      var ctor = typeof genFun === "function" && genFun.constructor;
      return ctor ? ctor === GeneratorFunction || // For the native GeneratorFunction constructor, the best we can
      // do is to check its .name property.
      (ctor.displayName || ctor.name) === "GeneratorFunction" : false;
    };

    exports.mark = function (genFun) {
      if (Object.setPrototypeOf) {
        Object.setPrototypeOf(genFun, GeneratorFunctionPrototype);
      } else {
        genFun.__proto__ = GeneratorFunctionPrototype;

        if (!(toStringTagSymbol in genFun)) {
          genFun[toStringTagSymbol] = "GeneratorFunction";
        }
      }

      genFun.prototype = Object.create(Gp);
      return genFun;
    }; // Within the body of any async function, `await x` is transformed to
    // `yield regeneratorRuntime.awrap(x)`, so that the runtime can test
    // `hasOwn.call(value, "__await")` to determine if the yielded value is
    // meant to be awaited.


    exports.awrap = function (arg) {
      return {
        __await: arg
      };
    };

    function AsyncIterator(generator) {
      function invoke(method, arg, resolve, reject) {
        var record = tryCatch(generator[method], generator, arg);

        if (record.type === "throw") {
          reject(record.arg);
        } else {
          var result = record.arg;
          var value = result.value;

          if (value && typeof value === "object" && hasOwn.call(value, "__await")) {
            return Promise.resolve(value.__await).then(function (value) {
              invoke("next", value, resolve, reject);
            }, function (err) {
              invoke("throw", err, resolve, reject);
            });
          }

          return Promise.resolve(value).then(function (unwrapped) {
            // When a yielded Promise is resolved, its final value becomes
            // the .value of the Promise<{value,done}> result for the
            // current iteration.
            result.value = unwrapped;
            resolve(result);
          }, function (error) {
            // If a rejected Promise was yielded, throw the rejection back
            // into the async generator function so it can be handled there.
            return invoke("throw", error, resolve, reject);
          });
        }
      }

      var previousPromise;

      function enqueue(method, arg) {
        function callInvokeWithMethodAndArg() {
          return new Promise(function (resolve, reject) {
            invoke(method, arg, resolve, reject);
          });
        }

        return previousPromise = // If enqueue has been called before, then we want to wait until
        // all previous Promises have been resolved before calling invoke,
        // so that results are always delivered in the correct order. If
        // enqueue has not been called before, then it is important to
        // call invoke immediately, without waiting on a callback to fire,
        // so that the async generator function has the opportunity to do
        // any necessary setup in a predictable way. This predictability
        // is why the Promise constructor synchronously invokes its
        // executor callback, and why async functions synchronously
        // execute code before the first await. Since we implement simple
        // async functions in terms of async generators, it is especially
        // important to get this right, even though it requires care.
        previousPromise ? previousPromise.then(callInvokeWithMethodAndArg, // Avoid propagating failures to Promises returned by later
        // invocations of the iterator.
        callInvokeWithMethodAndArg) : callInvokeWithMethodAndArg();
      } // Define the unified helper method that is used to implement .next,
      // .throw, and .return (see defineIteratorMethods).


      this._invoke = enqueue;
    }

    defineIteratorMethods(AsyncIterator.prototype);

    AsyncIterator.prototype[asyncIteratorSymbol] = function () {
      return this;
    };

    exports.AsyncIterator = AsyncIterator; // Note that simple async functions are implemented on top of
    // AsyncIterator objects; they just return a Promise for the value of
    // the final result produced by the iterator.

    exports.async = function (innerFn, outerFn, self, tryLocsList) {
      var iter = new AsyncIterator(wrap(innerFn, outerFn, self, tryLocsList));
      return exports.isGeneratorFunction(outerFn) ? iter // If outerFn is a generator, return the full iterator.
      : iter.next().then(function (result) {
        return result.done ? result.value : iter.next();
      });
    };

    function makeInvokeMethod(innerFn, self, context) {
      var state = GenStateSuspendedStart;
      return function invoke(method, arg) {
        if (state === GenStateExecuting) {
          throw new Error("Generator is already running");
        }

        if (state === GenStateCompleted) {
          if (method === "throw") {
            throw arg;
          } // Be forgiving, per 25.3.3.3.3 of the spec:
          // https://people.mozilla.org/~jorendorff/es6-draft.html#sec-generatorresume


          return doneResult();
        }

        context.method = method;
        context.arg = arg;

        while (true) {
          var delegate = context.delegate;

          if (delegate) {
            var delegateResult = maybeInvokeDelegate(delegate, context);

            if (delegateResult) {
              if (delegateResult === ContinueSentinel) continue;
              return delegateResult;
            }
          }

          if (context.method === "next") {
            // Setting context._sent for legacy support of Babel's
            // function.sent implementation.
            context.sent = context._sent = context.arg;
          } else if (context.method === "throw") {
            if (state === GenStateSuspendedStart) {
              state = GenStateCompleted;
              throw context.arg;
            }

            context.dispatchException(context.arg);
          } else if (context.method === "return") {
            context.abrupt("return", context.arg);
          }

          state = GenStateExecuting;
          var record = tryCatch(innerFn, self, context);

          if (record.type === "normal") {
            // If an exception is thrown from innerFn, we leave state ===
            // GenStateExecuting and loop back for another invocation.
            state = context.done ? GenStateCompleted : GenStateSuspendedYield;

            if (record.arg === ContinueSentinel) {
              continue;
            }

            return {
              value: record.arg,
              done: context.done
            };
          } else if (record.type === "throw") {
            state = GenStateCompleted; // Dispatch the exception by looping back around to the
            // context.dispatchException(context.arg) call above.

            context.method = "throw";
            context.arg = record.arg;
          }
        }
      };
    } // Call delegate.iterator[context.method](context.arg) and handle the
    // result, either by returning a { value, done } result from the
    // delegate iterator, or by modifying context.method and context.arg,
    // setting context.delegate to null, and returning the ContinueSentinel.


    function maybeInvokeDelegate(delegate, context) {
      var method = delegate.iterator[context.method];

      if (method === undefined$1) {
        // A .throw or .return when the delegate iterator has no .throw
        // method always terminates the yield* loop.
        context.delegate = null;

        if (context.method === "throw") {
          // Note: ["return"] must be used for ES3 parsing compatibility.
          if (delegate.iterator["return"]) {
            // If the delegate iterator has a return method, give it a
            // chance to clean up.
            context.method = "return";
            context.arg = undefined$1;
            maybeInvokeDelegate(delegate, context);

            if (context.method === "throw") {
              // If maybeInvokeDelegate(context) changed context.method from
              // "return" to "throw", let that override the TypeError below.
              return ContinueSentinel;
            }
          }

          context.method = "throw";
          context.arg = new TypeError("The iterator does not provide a 'throw' method");
        }

        return ContinueSentinel;
      }

      var record = tryCatch(method, delegate.iterator, context.arg);

      if (record.type === "throw") {
        context.method = "throw";
        context.arg = record.arg;
        context.delegate = null;
        return ContinueSentinel;
      }

      var info = record.arg;

      if (!info) {
        context.method = "throw";
        context.arg = new TypeError("iterator result is not an object");
        context.delegate = null;
        return ContinueSentinel;
      }

      if (info.done) {
        // Assign the result of the finished delegate to the temporary
        // variable specified by delegate.resultName (see delegateYield).
        context[delegate.resultName] = info.value; // Resume execution at the desired location (see delegateYield).

        context.next = delegate.nextLoc; // If context.method was "throw" but the delegate handled the
        // exception, let the outer generator proceed normally. If
        // context.method was "next", forget context.arg since it has been
        // "consumed" by the delegate iterator. If context.method was
        // "return", allow the original .return call to continue in the
        // outer generator.

        if (context.method !== "return") {
          context.method = "next";
          context.arg = undefined$1;
        }
      } else {
        // Re-yield the result returned by the delegate method.
        return info;
      } // The delegate iterator is finished, so forget it and continue with
      // the outer generator.


      context.delegate = null;
      return ContinueSentinel;
    } // Define Generator.prototype.{next,throw,return} in terms of the
    // unified ._invoke helper method.


    defineIteratorMethods(Gp);
    Gp[toStringTagSymbol] = "Generator"; // A Generator should always return itself as the iterator object when the
    // @@iterator function is called on it. Some browsers' implementations of the
    // iterator prototype chain incorrectly implement this, causing the Generator
    // object to not be returned from this call. This ensures that doesn't happen.
    // See https://github.com/facebook/regenerator/issues/274 for more details.

    Gp[iteratorSymbol] = function () {
      return this;
    };

    Gp.toString = function () {
      return "[object Generator]";
    };

    function pushTryEntry(locs) {
      var entry = {
        tryLoc: locs[0]
      };

      if (1 in locs) {
        entry.catchLoc = locs[1];
      }

      if (2 in locs) {
        entry.finallyLoc = locs[2];
        entry.afterLoc = locs[3];
      }

      this.tryEntries.push(entry);
    }

    function resetTryEntry(entry) {
      var record = entry.completion || {};
      record.type = "normal";
      delete record.arg;
      entry.completion = record;
    }

    function Context(tryLocsList) {
      // The root entry object (effectively a try statement without a catch
      // or a finally block) gives us a place to store values thrown from
      // locations where there is no enclosing try statement.
      this.tryEntries = [{
        tryLoc: "root"
      }];
      tryLocsList.forEach(pushTryEntry, this);
      this.reset(true);
    }

    exports.keys = function (object) {
      var keys = [];

      for (var key in object) {
        keys.push(key);
      }

      keys.reverse(); // Rather than returning an object with a next method, we keep
      // things simple and return the next function itself.

      return function next() {
        while (keys.length) {
          var key = keys.pop();

          if (key in object) {
            next.value = key;
            next.done = false;
            return next;
          }
        } // To avoid creating an additional object, we just hang the .value
        // and .done properties off the next function object itself. This
        // also ensures that the minifier will not anonymize the function.


        next.done = true;
        return next;
      };
    };

    function values(iterable) {
      if (iterable) {
        var iteratorMethod = iterable[iteratorSymbol];

        if (iteratorMethod) {
          return iteratorMethod.call(iterable);
        }

        if (typeof iterable.next === "function") {
          return iterable;
        }

        if (!isNaN(iterable.length)) {
          var i = -1,
              next = function next() {
            while (++i < iterable.length) {
              if (hasOwn.call(iterable, i)) {
                next.value = iterable[i];
                next.done = false;
                return next;
              }
            }

            next.value = undefined$1;
            next.done = true;
            return next;
          };

          return next.next = next;
        }
      } // Return an iterator with no values.


      return {
        next: doneResult
      };
    }

    exports.values = values;

    function doneResult() {
      return {
        value: undefined$1,
        done: true
      };
    }

    Context.prototype = {
      constructor: Context,
      reset: function (skipTempReset) {
        this.prev = 0;
        this.next = 0; // Resetting context._sent for legacy support of Babel's
        // function.sent implementation.

        this.sent = this._sent = undefined$1;
        this.done = false;
        this.delegate = null;
        this.method = "next";
        this.arg = undefined$1;
        this.tryEntries.forEach(resetTryEntry);

        if (!skipTempReset) {
          for (var name in this) {
            // Not sure about the optimal order of these conditions:
            if (name.charAt(0) === "t" && hasOwn.call(this, name) && !isNaN(+name.slice(1))) {
              this[name] = undefined$1;
            }
          }
        }
      },
      stop: function () {
        this.done = true;
        var rootEntry = this.tryEntries[0];
        var rootRecord = rootEntry.completion;

        if (rootRecord.type === "throw") {
          throw rootRecord.arg;
        }

        return this.rval;
      },
      dispatchException: function (exception) {
        if (this.done) {
          throw exception;
        }

        var context = this;

        function handle(loc, caught) {
          record.type = "throw";
          record.arg = exception;
          context.next = loc;

          if (caught) {
            // If the dispatched exception was caught by a catch block,
            // then let that catch block handle the exception normally.
            context.method = "next";
            context.arg = undefined$1;
          }

          return !!caught;
        }

        for (var i = this.tryEntries.length - 1; i >= 0; --i) {
          var entry = this.tryEntries[i];
          var record = entry.completion;

          if (entry.tryLoc === "root") {
            // Exception thrown outside of any try block that could handle
            // it, so set the completion value of the entire function to
            // throw the exception.
            return handle("end");
          }

          if (entry.tryLoc <= this.prev) {
            var hasCatch = hasOwn.call(entry, "catchLoc");
            var hasFinally = hasOwn.call(entry, "finallyLoc");

            if (hasCatch && hasFinally) {
              if (this.prev < entry.catchLoc) {
                return handle(entry.catchLoc, true);
              } else if (this.prev < entry.finallyLoc) {
                return handle(entry.finallyLoc);
              }
            } else if (hasCatch) {
              if (this.prev < entry.catchLoc) {
                return handle(entry.catchLoc, true);
              }
            } else if (hasFinally) {
              if (this.prev < entry.finallyLoc) {
                return handle(entry.finallyLoc);
              }
            } else {
              throw new Error("try statement without catch or finally");
            }
          }
        }
      },
      abrupt: function (type, arg) {
        for (var i = this.tryEntries.length - 1; i >= 0; --i) {
          var entry = this.tryEntries[i];

          if (entry.tryLoc <= this.prev && hasOwn.call(entry, "finallyLoc") && this.prev < entry.finallyLoc) {
            var finallyEntry = entry;
            break;
          }
        }

        if (finallyEntry && (type === "break" || type === "continue") && finallyEntry.tryLoc <= arg && arg <= finallyEntry.finallyLoc) {
          // Ignore the finally entry if control is not jumping to a
          // location outside the try/catch block.
          finallyEntry = null;
        }

        var record = finallyEntry ? finallyEntry.completion : {};
        record.type = type;
        record.arg = arg;

        if (finallyEntry) {
          this.method = "next";
          this.next = finallyEntry.finallyLoc;
          return ContinueSentinel;
        }

        return this.complete(record);
      },
      complete: function (record, afterLoc) {
        if (record.type === "throw") {
          throw record.arg;
        }

        if (record.type === "break" || record.type === "continue") {
          this.next = record.arg;
        } else if (record.type === "return") {
          this.rval = this.arg = record.arg;
          this.method = "return";
          this.next = "end";
        } else if (record.type === "normal" && afterLoc) {
          this.next = afterLoc;
        }

        return ContinueSentinel;
      },
      finish: function (finallyLoc) {
        for (var i = this.tryEntries.length - 1; i >= 0; --i) {
          var entry = this.tryEntries[i];

          if (entry.finallyLoc === finallyLoc) {
            this.complete(entry.completion, entry.afterLoc);
            resetTryEntry(entry);
            return ContinueSentinel;
          }
        }
      },
      "catch": function (tryLoc) {
        for (var i = this.tryEntries.length - 1; i >= 0; --i) {
          var entry = this.tryEntries[i];

          if (entry.tryLoc === tryLoc) {
            var record = entry.completion;

            if (record.type === "throw") {
              var thrown = record.arg;
              resetTryEntry(entry);
            }

            return thrown;
          }
        } // The context.catch method must only be called with a location
        // argument that corresponds to a known catch block.


        throw new Error("illegal catch attempt");
      },
      delegateYield: function (iterable, resultName, nextLoc) {
        this.delegate = {
          iterator: values(iterable),
          resultName: resultName,
          nextLoc: nextLoc
        };

        if (this.method === "next") {
          // Deliberately forget the last sent value so that we don't
          // accidentally pass it on to the delegate.
          this.arg = undefined$1;
        }

        return ContinueSentinel;
      }
    }; // Regardless of whether this script is executing as a CommonJS module
    // or not, return the runtime object so that we can declare the variable
    // regeneratorRuntime in the outer scope, which allows this module to be
    // injected easily by `bin/regenerator --include-runtime script.js`.

    return exports;
  }( // If this script is executing as a CommonJS module, use module.exports
  // as the regeneratorRuntime namespace. Otherwise create a new empty
  // object. Either way, the resulting object will be used to initialize
  // the regeneratorRuntime variable at the top of this file.
   module.exports );

  try {
    regeneratorRuntime = runtime;
  } catch (accidentalStrictMode) {
    // This module should not be running in strict mode, so the above
    // assignment should always work unless something is misconfigured. Just
    // in case runtime.js accidentally runs in strict mode, we can escape
    // strict mode using a global Function call. This could conceivably fail
    // if a Content Security Policy forbids using Function, but in that case
    // the proper solution is to fix the accidental strict mode problem. If
    // you've misconfigured your bundler to force strict mode and applied a
    // CSP to forbid Function, and you're not willing to fix either of those
    // problems, please detail your unique predicament in a GitHub issue.
    Function("r", "regeneratorRuntime = r")(runtime);
  }
});

var C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator = runtime_1;

function _typeof(obj) {
  if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") {
    _typeof = function (obj) {
      return typeof obj;
    };
  } else {
    _typeof = function (obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj;
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _possibleConstructorReturn(self, call) {
  if (call && (typeof call === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _toConsumableArray(arr) {
  return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread();
}

function _arrayWithoutHoles(arr) {
  if (Array.isArray(arr)) {
    for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) arr2[i] = arr[i];

    return arr2;
  }
}

function _iterableToArray(iter) {
  if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter);
}

function _nonIterableSpread() {
  throw new TypeError("Invalid attempt to spread non-iterable instance");
}

var iterators = {};

var correctPrototypeGetter = !fails(function () {
  function F() {
    /* empty */
  }

  F.prototype.constructor = null;
  return Object.getPrototypeOf(new F()) !== F.prototype;
});

var IE_PROTO$1 = sharedKey('IE_PROTO');
var ObjectPrototype$1 = Object.prototype; // `Object.getPrototypeOf` method
// https://tc39.github.io/ecma262/#sec-object.getprototypeof

var objectGetPrototypeOf = correctPrototypeGetter ? Object.getPrototypeOf : function (O) {
  O = toObject(O);
  if (has(O, IE_PROTO$1)) return O[IE_PROTO$1];

  if (typeof O.constructor == 'function' && O instanceof O.constructor) {
    return O.constructor.prototype;
  }

  return O instanceof Object ? ObjectPrototype$1 : null;
};

var ITERATOR = wellKnownSymbol('iterator');
var BUGGY_SAFARI_ITERATORS = false;
// https://tc39.github.io/ecma262/#sec-%iteratorprototype%-object


var IteratorPrototype, PrototypeOfArrayIteratorPrototype, arrayIterator;

if ([].keys) {
  arrayIterator = [].keys(); // Safari 8 has buggy iterators w/o `next`

  if (!('next' in arrayIterator)) BUGGY_SAFARI_ITERATORS = true;else {
    PrototypeOfArrayIteratorPrototype = objectGetPrototypeOf(objectGetPrototypeOf(arrayIterator));
    if (PrototypeOfArrayIteratorPrototype !== Object.prototype) IteratorPrototype = PrototypeOfArrayIteratorPrototype;
  }
}

if (IteratorPrototype == undefined) IteratorPrototype = {}; // 25.1.2.1.1 %IteratorPrototype%[@@iterator]()

var iteratorsCore = {
  IteratorPrototype: IteratorPrototype,
  BUGGY_SAFARI_ITERATORS: BUGGY_SAFARI_ITERATORS
};

var IteratorPrototype$1 = iteratorsCore.IteratorPrototype;

var returnThis = function () {
  return this;
};

var createIteratorConstructor = function (IteratorConstructor, NAME, next) {
  var TO_STRING_TAG = NAME + ' Iterator';
  IteratorConstructor.prototype = objectCreate(IteratorPrototype$1, {
    next: createPropertyDescriptor(1, next)
  });
  setToStringTag(IteratorConstructor, TO_STRING_TAG, false, true);
  iterators[TO_STRING_TAG] = returnThis;
  return IteratorConstructor;
};

var aPossiblePrototype = function (it) {
  if (!isObject(it) && it !== null) {
    throw TypeError("Can't set " + String(it) + ' as a prototype');
  }

  return it;
};

// https://tc39.github.io/ecma262/#sec-object.setprototypeof
// Works with __proto__ only. Old v8 can't work with null proto objects.

/* eslint-disable no-proto */

var objectSetPrototypeOf = Object.setPrototypeOf || ('__proto__' in {} ? function () {
  var CORRECT_SETTER = false;
  var test = {};
  var setter;

  try {
    setter = Object.getOwnPropertyDescriptor(Object.prototype, '__proto__').set;
    setter.call(test, []);
    CORRECT_SETTER = test instanceof Array;
  } catch (error) {
    /* empty */
  }

  return function setPrototypeOf(O, proto) {
    anObject(O);
    aPossiblePrototype(proto);
    if (CORRECT_SETTER) setter.call(O, proto);else O.__proto__ = proto;
    return O;
  };
}() : undefined);

var IteratorPrototype$2 = iteratorsCore.IteratorPrototype;
var BUGGY_SAFARI_ITERATORS$1 = iteratorsCore.BUGGY_SAFARI_ITERATORS;
var ITERATOR$1 = wellKnownSymbol('iterator');
var KEYS = 'keys';
var VALUES = 'values';
var ENTRIES = 'entries';

var returnThis$1 = function () {
  return this;
};

var defineIterator = function (Iterable, NAME, IteratorConstructor, next, DEFAULT, IS_SET, FORCED) {
  createIteratorConstructor(IteratorConstructor, NAME, next);

  var getIterationMethod = function (KIND) {
    if (KIND === DEFAULT && defaultIterator) return defaultIterator;
    if (!BUGGY_SAFARI_ITERATORS$1 && KIND in IterablePrototype) return IterablePrototype[KIND];

    switch (KIND) {
      case KEYS:
        return function keys() {
          return new IteratorConstructor(this, KIND);
        };

      case VALUES:
        return function values() {
          return new IteratorConstructor(this, KIND);
        };

      case ENTRIES:
        return function entries() {
          return new IteratorConstructor(this, KIND);
        };
    }

    return function () {
      return new IteratorConstructor(this);
    };
  };

  var TO_STRING_TAG = NAME + ' Iterator';
  var INCORRECT_VALUES_NAME = false;
  var IterablePrototype = Iterable.prototype;
  var nativeIterator = IterablePrototype[ITERATOR$1] || IterablePrototype['@@iterator'] || DEFAULT && IterablePrototype[DEFAULT];
  var defaultIterator = !BUGGY_SAFARI_ITERATORS$1 && nativeIterator || getIterationMethod(DEFAULT);
  var anyNativeIterator = NAME == 'Array' ? IterablePrototype.entries || nativeIterator : nativeIterator;
  var CurrentIteratorPrototype, methods, KEY; // fix native

  if (anyNativeIterator) {
    CurrentIteratorPrototype = objectGetPrototypeOf(anyNativeIterator.call(new Iterable()));

    if (IteratorPrototype$2 !== Object.prototype && CurrentIteratorPrototype.next) {


      setToStringTag(CurrentIteratorPrototype, TO_STRING_TAG, true, true);
      iterators[TO_STRING_TAG] = returnThis$1;
    }
  } // fix Array#{values, @@iterator}.name in V8 / FF


  if (DEFAULT == VALUES && nativeIterator && nativeIterator.name !== VALUES) {
    INCORRECT_VALUES_NAME = true;

    defaultIterator = function values() {
      return nativeIterator.call(this);
    };
  } // define iterator


  if (( FORCED) && IterablePrototype[ITERATOR$1] !== defaultIterator) {
    createNonEnumerableProperty(IterablePrototype, ITERATOR$1, defaultIterator);
  }

  iterators[NAME] = defaultIterator; // export additional methods

  if (DEFAULT) {
    methods = {
      values: getIterationMethod(VALUES),
      keys: IS_SET ? defaultIterator : getIterationMethod(KEYS),
      entries: getIterationMethod(ENTRIES)
    };
    if (FORCED) for (KEY in methods) {
      if (BUGGY_SAFARI_ITERATORS$1 || INCORRECT_VALUES_NAME || !(KEY in IterablePrototype)) {
        redefine(IterablePrototype, KEY, methods[KEY]);
      }
    } else _export({
      target: NAME,
      proto: true,
      forced: BUGGY_SAFARI_ITERATORS$1 || INCORRECT_VALUES_NAME
    }, methods);
  }

  return methods;
};

var ARRAY_ITERATOR = 'Array Iterator';
var setInternalState$1 = internalState.set;
var getInternalState$1 = internalState.getterFor(ARRAY_ITERATOR); // `Array.prototype.entries` method
// https://tc39.github.io/ecma262/#sec-array.prototype.entries
// `Array.prototype.keys` method
// https://tc39.github.io/ecma262/#sec-array.prototype.keys
// `Array.prototype.values` method
// https://tc39.github.io/ecma262/#sec-array.prototype.values
// `Array.prototype[@@iterator]` method
// https://tc39.github.io/ecma262/#sec-array.prototype-@@iterator
// `CreateArrayIterator` internal method
// https://tc39.github.io/ecma262/#sec-createarrayiterator

var es_array_iterator = defineIterator(Array, 'Array', function (iterated, kind) {
  setInternalState$1(this, {
    type: ARRAY_ITERATOR,
    target: toIndexedObject(iterated),
    // target
    index: 0,
    // next index
    kind: kind // kind

  }); // `%ArrayIteratorPrototype%.next` method
  // https://tc39.github.io/ecma262/#sec-%arrayiteratorprototype%.next
}, function () {
  var state = getInternalState$1(this);
  var target = state.target;
  var kind = state.kind;
  var index = state.index++;

  if (!target || index >= target.length) {
    state.target = undefined;
    return {
      value: undefined,
      done: true
    };
  }

  if (kind == 'keys') return {
    value: index,
    done: false
  };
  if (kind == 'values') return {
    value: target[index],
    done: false
  };
  return {
    value: [index, target[index]],
    done: false
  };
}, 'values'); // argumentsList[@@iterator] is %ArrayProto_values%
// https://tc39.github.io/ecma262/#sec-createunmappedargumentsobject
// https://tc39.github.io/ecma262/#sec-createmappedargumentsobject

iterators.Arguments = iterators.Array; // https://tc39.github.io/ecma262/#sec-array.prototype-@@unscopables

// iterable DOM collections
// flag - `iterable` interface - 'entries', 'keys', 'values', 'forEach' methods
var domIterables = {
  CSSRuleList: 0,
  CSSStyleDeclaration: 0,
  CSSValueList: 0,
  ClientRectList: 0,
  DOMRectList: 0,
  DOMStringList: 0,
  DOMTokenList: 1,
  DataTransferItemList: 0,
  FileList: 0,
  HTMLAllCollection: 0,
  HTMLCollection: 0,
  HTMLFormElement: 0,
  HTMLSelectElement: 0,
  MediaList: 0,
  MimeTypeArray: 0,
  NamedNodeMap: 0,
  NodeList: 1,
  PaintRequestList: 0,
  Plugin: 0,
  PluginArray: 0,
  SVGLengthList: 0,
  SVGNumberList: 0,
  SVGPathSegList: 0,
  SVGPointList: 0,
  SVGStringList: 0,
  SVGTransformList: 0,
  SourceBufferList: 0,
  StyleSheetList: 0,
  TextTrackCueList: 0,
  TextTrackList: 0,
  TouchList: 0
};

var TO_STRING_TAG$3 = wellKnownSymbol('toStringTag');

for (var COLLECTION_NAME in domIterables) {
  var Collection = global_1[COLLECTION_NAME];
  var CollectionPrototype = Collection && Collection.prototype;

  if (CollectionPrototype && classof(CollectionPrototype) !== TO_STRING_TAG$3) {
    createNonEnumerableProperty(CollectionPrototype, TO_STRING_TAG$3, COLLECTION_NAME);
  }

  iterators[COLLECTION_NAME] = iterators.Array;
}

var createMethod$2 = function (CONVERT_TO_STRING) {
  return function ($this, pos) {
    var S = String(requireObjectCoercible($this));
    var position = toInteger(pos);
    var size = S.length;
    var first, second;
    if (position < 0 || position >= size) return CONVERT_TO_STRING ? '' : undefined;
    first = S.charCodeAt(position);
    return first < 0xD800 || first > 0xDBFF || position + 1 === size || (second = S.charCodeAt(position + 1)) < 0xDC00 || second > 0xDFFF ? CONVERT_TO_STRING ? S.charAt(position) : first : CONVERT_TO_STRING ? S.slice(position, position + 2) : (first - 0xD800 << 10) + (second - 0xDC00) + 0x10000;
  };
};

var stringMultibyte = {
  // `String.prototype.codePointAt` method
  // https://tc39.github.io/ecma262/#sec-string.prototype.codepointat
  codeAt: createMethod$2(false),
  // `String.prototype.at` method
  // https://github.com/mathiasbynens/String.prototype.at
  charAt: createMethod$2(true)
};

var charAt = stringMultibyte.charAt;
var STRING_ITERATOR = 'String Iterator';
var setInternalState$2 = internalState.set;
var getInternalState$2 = internalState.getterFor(STRING_ITERATOR); // `String.prototype[@@iterator]` method
// https://tc39.github.io/ecma262/#sec-string.prototype-@@iterator

defineIterator(String, 'String', function (iterated) {
  setInternalState$2(this, {
    type: STRING_ITERATOR,
    string: String(iterated),
    index: 0
  }); // `%StringIteratorPrototype%.next` method
  // https://tc39.github.io/ecma262/#sec-%stringiteratorprototype%.next
}, function next() {
  var state = getInternalState$2(this);
  var string = state.string;
  var index = state.index;
  var point;
  if (index >= string.length) return {
    value: undefined,
    done: true
  };
  point = charAt(string, index);
  state.index += point.length;
  return {
    value: point,
    done: false
  };
});

var ITERATOR$2 = wellKnownSymbol('iterator');

var getIteratorMethod = function (it) {
  if (it != undefined) return it[ITERATOR$2] || it['@@iterator'] || iterators[classof(it)];
};

var getIterator = function (it) {
  var iteratorMethod = getIteratorMethod(it);

  if (typeof iteratorMethod != 'function') {
    throw TypeError(String(it) + ' is not iterable');
  }

  return anObject(iteratorMethod.call(it));
};

var getIterator_1 = getIterator;

var getIterator$1 = getIterator_1;

/*! *****************************************************************************
Copyright (C) Microsoft. All rights reserved.
Licensed under the Apache License, Version 2.0 (the "License"); you may not use
this file except in compliance with the License. You may obtain a copy of the
License at http://www.apache.org/licenses/LICENSE-2.0

THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
MERCHANTABLITY OR NON-INFRINGEMENT.

See the Apache Version 2.0 License for specific language governing permissions
and limitations under the License.
***************************************************************************** */

var Reflect$1;

(function (Reflect) {
  // Metadata Proposal
  // https://rbuckton.github.io/reflect-metadata/
  (function (factory) {
    var root = typeof commonjsGlobal === "object" ? commonjsGlobal : typeof self === "object" ? self : typeof this === "object" ? this : Function("return this;")();
    var exporter = makeExporter(Reflect);

    if (typeof root.Reflect === "undefined") {
      root.Reflect = Reflect;
    } else {
      exporter = makeExporter(root.Reflect, exporter);
    }

    factory(exporter);

    function makeExporter(target, previous) {
      return function (key, value) {
        if (typeof target[key] !== "function") {
          Object.defineProperty(target, key, {
            configurable: true,
            writable: true,
            value: value
          });
        }

        if (previous) previous(key, value);
      };
    }
  })(function (exporter) {
    var hasOwn = Object.prototype.hasOwnProperty; // feature test for Symbol support

    var supportsSymbol = typeof Symbol === "function";
    var toPrimitiveSymbol = supportsSymbol && typeof Symbol.toPrimitive !== "undefined" ? Symbol.toPrimitive : "@@toPrimitive";
    var iteratorSymbol = supportsSymbol && typeof Symbol.iterator !== "undefined" ? Symbol.iterator : "@@iterator";
    var supportsCreate = typeof Object.create === "function"; // feature test for Object.create support

    var supportsProto = {
      __proto__: []
    } instanceof Array; // feature test for __proto__ support

    var downLevel = !supportsCreate && !supportsProto;
    var HashMap = {
      // create an object in dictionary mode (a.k.a. "slow" mode in v8)
      create: supportsCreate ? function () {
        return MakeDictionary(Object.create(null));
      } : supportsProto ? function () {
        return MakeDictionary({
          __proto__: null
        });
      } : function () {
        return MakeDictionary({});
      },
      has: downLevel ? function (map, key) {
        return hasOwn.call(map, key);
      } : function (map, key) {
        return key in map;
      },
      get: downLevel ? function (map, key) {
        return hasOwn.call(map, key) ? map[key] : undefined;
      } : function (map, key) {
        return map[key];
      }
    }; // Load global or shim versions of Map, Set, and WeakMap

    var functionPrototype = Object.getPrototypeOf(Function);
    var usePolyfill = typeof process === "object" && process.env && process.env["REFLECT_METADATA_USE_MAP_POLYFILL"] === "true";

    var _Map = !usePolyfill && typeof Map === "function" && typeof Map.prototype.entries === "function" ? Map : CreateMapPolyfill();

    var _Set = !usePolyfill && typeof Set === "function" && typeof Set.prototype.entries === "function" ? Set : CreateSetPolyfill();

    var _WeakMap = !usePolyfill && typeof WeakMap === "function" ? WeakMap : CreateWeakMapPolyfill(); // [[Metadata]] internal slot
    // https://rbuckton.github.io/reflect-metadata/#ordinary-object-internal-methods-and-internal-slots


    var Metadata = new _WeakMap();
    /**
     * Applies a set of decorators to a property of a target object.
     * @param decorators An array of decorators.
     * @param target The target object.
     * @param propertyKey (Optional) The property key to decorate.
     * @param attributes (Optional) The property descriptor for the target key.
     * @remarks Decorators are applied in reverse order.
     * @example
     *
     *     class Example {
     *         // property declarations are not part of ES6, though they are valid in TypeScript:
     *         // static staticProperty;
     *         // property;
     *
     *         constructor(p) { }
     *         static staticMethod(p) { }
     *         method(p) { }
     *     }
     *
     *     // constructor
     *     Example = Reflect.decorate(decoratorsArray, Example);
     *
     *     // property (on constructor)
     *     Reflect.decorate(decoratorsArray, Example, "staticProperty");
     *
     *     // property (on prototype)
     *     Reflect.decorate(decoratorsArray, Example.prototype, "property");
     *
     *     // method (on constructor)
     *     Object.defineProperty(Example, "staticMethod",
     *         Reflect.decorate(decoratorsArray, Example, "staticMethod",
     *             Object.getOwnPropertyDescriptor(Example, "staticMethod")));
     *
     *     // method (on prototype)
     *     Object.defineProperty(Example.prototype, "method",
     *         Reflect.decorate(decoratorsArray, Example.prototype, "method",
     *             Object.getOwnPropertyDescriptor(Example.prototype, "method")));
     *
     */

    function decorate(decorators, target, propertyKey, attributes) {
      if (!IsUndefined(propertyKey)) {
        if (!IsArray(decorators)) throw new TypeError();
        if (!IsObject(target)) throw new TypeError();
        if (!IsObject(attributes) && !IsUndefined(attributes) && !IsNull(attributes)) throw new TypeError();
        if (IsNull(attributes)) attributes = undefined;
        propertyKey = ToPropertyKey(propertyKey);
        return DecorateProperty(decorators, target, propertyKey, attributes);
      } else {
        if (!IsArray(decorators)) throw new TypeError();
        if (!IsConstructor(target)) throw new TypeError();
        return DecorateConstructor(decorators, target);
      }
    }

    exporter("decorate", decorate); // 4.1.2 Reflect.metadata(metadataKey, metadataValue)
    // https://rbuckton.github.io/reflect-metadata/#reflect.metadata

    /**
     * A default metadata decorator factory that can be used on a class, class member, or parameter.
     * @param metadataKey The key for the metadata entry.
     * @param metadataValue The value for the metadata entry.
     * @returns A decorator function.
     * @remarks
     * If `metadataKey` is already defined for the target and target key, the
     * metadataValue for that key will be overwritten.
     * @example
     *
     *     // constructor
     *     @Reflect.metadata(key, value)
     *     class Example {
     *     }
     *
     *     // property (on constructor, TypeScript only)
     *     class Example {
     *         @Reflect.metadata(key, value)
     *         static staticProperty;
     *     }
     *
     *     // property (on prototype, TypeScript only)
     *     class Example {
     *         @Reflect.metadata(key, value)
     *         property;
     *     }
     *
     *     // method (on constructor)
     *     class Example {
     *         @Reflect.metadata(key, value)
     *         static staticMethod() { }
     *     }
     *
     *     // method (on prototype)
     *     class Example {
     *         @Reflect.metadata(key, value)
     *         method() { }
     *     }
     *
     */

    function metadata(metadataKey, metadataValue) {
      function decorator(target, propertyKey) {
        if (!IsObject(target)) throw new TypeError();
        if (!IsUndefined(propertyKey) && !IsPropertyKey(propertyKey)) throw new TypeError();
        OrdinaryDefineOwnMetadata(metadataKey, metadataValue, target, propertyKey);
      }

      return decorator;
    }

    exporter("metadata", metadata);
    /**
     * Define a unique metadata entry on the target.
     * @param metadataKey A key used to store and retrieve metadata.
     * @param metadataValue A value that contains attached metadata.
     * @param target The target object on which to define metadata.
     * @param propertyKey (Optional) The property key for the target.
     * @example
     *
     *     class Example {
     *         // property declarations are not part of ES6, though they are valid in TypeScript:
     *         // static staticProperty;
     *         // property;
     *
     *         constructor(p) { }
     *         static staticMethod(p) { }
     *         method(p) { }
     *     }
     *
     *     // constructor
     *     Reflect.defineMetadata("custom:annotation", options, Example);
     *
     *     // property (on constructor)
     *     Reflect.defineMetadata("custom:annotation", options, Example, "staticProperty");
     *
     *     // property (on prototype)
     *     Reflect.defineMetadata("custom:annotation", options, Example.prototype, "property");
     *
     *     // method (on constructor)
     *     Reflect.defineMetadata("custom:annotation", options, Example, "staticMethod");
     *
     *     // method (on prototype)
     *     Reflect.defineMetadata("custom:annotation", options, Example.prototype, "method");
     *
     *     // decorator factory as metadata-producing annotation.
     *     function MyAnnotation(options): Decorator {
     *         return (target, key?) => Reflect.defineMetadata("custom:annotation", options, target, key);
     *     }
     *
     */

    function defineMetadata(metadataKey, metadataValue, target, propertyKey) {
      if (!IsObject(target)) throw new TypeError();
      if (!IsUndefined(propertyKey)) propertyKey = ToPropertyKey(propertyKey);
      return OrdinaryDefineOwnMetadata(metadataKey, metadataValue, target, propertyKey);
    }

    exporter("defineMetadata", defineMetadata);
    /**
     * Gets a value indicating whether the target object or its prototype chain has the provided metadata key defined.
     * @param metadataKey A key used to store and retrieve metadata.
     * @param target The target object on which the metadata is defined.
     * @param propertyKey (Optional) The property key for the target.
     * @returns `true` if the metadata key was defined on the target object or its prototype chain; otherwise, `false`.
     * @example
     *
     *     class Example {
     *         // property declarations are not part of ES6, though they are valid in TypeScript:
     *         // static staticProperty;
     *         // property;
     *
     *         constructor(p) { }
     *         static staticMethod(p) { }
     *         method(p) { }
     *     }
     *
     *     // constructor
     *     result = Reflect.hasMetadata("custom:annotation", Example);
     *
     *     // property (on constructor)
     *     result = Reflect.hasMetadata("custom:annotation", Example, "staticProperty");
     *
     *     // property (on prototype)
     *     result = Reflect.hasMetadata("custom:annotation", Example.prototype, "property");
     *
     *     // method (on constructor)
     *     result = Reflect.hasMetadata("custom:annotation", Example, "staticMethod");
     *
     *     // method (on prototype)
     *     result = Reflect.hasMetadata("custom:annotation", Example.prototype, "method");
     *
     */

    function hasMetadata(metadataKey, target, propertyKey) {
      if (!IsObject(target)) throw new TypeError();
      if (!IsUndefined(propertyKey)) propertyKey = ToPropertyKey(propertyKey);
      return OrdinaryHasMetadata(metadataKey, target, propertyKey);
    }

    exporter("hasMetadata", hasMetadata);
    /**
     * Gets a value indicating whether the target object has the provided metadata key defined.
     * @param metadataKey A key used to store and retrieve metadata.
     * @param target The target object on which the metadata is defined.
     * @param propertyKey (Optional) The property key for the target.
     * @returns `true` if the metadata key was defined on the target object; otherwise, `false`.
     * @example
     *
     *     class Example {
     *         // property declarations are not part of ES6, though they are valid in TypeScript:
     *         // static staticProperty;
     *         // property;
     *
     *         constructor(p) { }
     *         static staticMethod(p) { }
     *         method(p) { }
     *     }
     *
     *     // constructor
     *     result = Reflect.hasOwnMetadata("custom:annotation", Example);
     *
     *     // property (on constructor)
     *     result = Reflect.hasOwnMetadata("custom:annotation", Example, "staticProperty");
     *
     *     // property (on prototype)
     *     result = Reflect.hasOwnMetadata("custom:annotation", Example.prototype, "property");
     *
     *     // method (on constructor)
     *     result = Reflect.hasOwnMetadata("custom:annotation", Example, "staticMethod");
     *
     *     // method (on prototype)
     *     result = Reflect.hasOwnMetadata("custom:annotation", Example.prototype, "method");
     *
     */

    function hasOwnMetadata(metadataKey, target, propertyKey) {
      if (!IsObject(target)) throw new TypeError();
      if (!IsUndefined(propertyKey)) propertyKey = ToPropertyKey(propertyKey);
      return OrdinaryHasOwnMetadata(metadataKey, target, propertyKey);
    }

    exporter("hasOwnMetadata", hasOwnMetadata);
    /**
     * Gets the metadata value for the provided metadata key on the target object or its prototype chain.
     * @param metadataKey A key used to store and retrieve metadata.
     * @param target The target object on which the metadata is defined.
     * @param propertyKey (Optional) The property key for the target.
     * @returns The metadata value for the metadata key if found; otherwise, `undefined`.
     * @example
     *
     *     class Example {
     *         // property declarations are not part of ES6, though they are valid in TypeScript:
     *         // static staticProperty;
     *         // property;
     *
     *         constructor(p) { }
     *         static staticMethod(p) { }
     *         method(p) { }
     *     }
     *
     *     // constructor
     *     result = Reflect.getMetadata("custom:annotation", Example);
     *
     *     // property (on constructor)
     *     result = Reflect.getMetadata("custom:annotation", Example, "staticProperty");
     *
     *     // property (on prototype)
     *     result = Reflect.getMetadata("custom:annotation", Example.prototype, "property");
     *
     *     // method (on constructor)
     *     result = Reflect.getMetadata("custom:annotation", Example, "staticMethod");
     *
     *     // method (on prototype)
     *     result = Reflect.getMetadata("custom:annotation", Example.prototype, "method");
     *
     */

    function getMetadata(metadataKey, target, propertyKey) {
      if (!IsObject(target)) throw new TypeError();
      if (!IsUndefined(propertyKey)) propertyKey = ToPropertyKey(propertyKey);
      return OrdinaryGetMetadata(metadataKey, target, propertyKey);
    }

    exporter("getMetadata", getMetadata);
    /**
     * Gets the metadata value for the provided metadata key on the target object.
     * @param metadataKey A key used to store and retrieve metadata.
     * @param target The target object on which the metadata is defined.
     * @param propertyKey (Optional) The property key for the target.
     * @returns The metadata value for the metadata key if found; otherwise, `undefined`.
     * @example
     *
     *     class Example {
     *         // property declarations are not part of ES6, though they are valid in TypeScript:
     *         // static staticProperty;
     *         // property;
     *
     *         constructor(p) { }
     *         static staticMethod(p) { }
     *         method(p) { }
     *     }
     *
     *     // constructor
     *     result = Reflect.getOwnMetadata("custom:annotation", Example);
     *
     *     // property (on constructor)
     *     result = Reflect.getOwnMetadata("custom:annotation", Example, "staticProperty");
     *
     *     // property (on prototype)
     *     result = Reflect.getOwnMetadata("custom:annotation", Example.prototype, "property");
     *
     *     // method (on constructor)
     *     result = Reflect.getOwnMetadata("custom:annotation", Example, "staticMethod");
     *
     *     // method (on prototype)
     *     result = Reflect.getOwnMetadata("custom:annotation", Example.prototype, "method");
     *
     */

    function getOwnMetadata(metadataKey, target, propertyKey) {
      if (!IsObject(target)) throw new TypeError();
      if (!IsUndefined(propertyKey)) propertyKey = ToPropertyKey(propertyKey);
      return OrdinaryGetOwnMetadata(metadataKey, target, propertyKey);
    }

    exporter("getOwnMetadata", getOwnMetadata);
    /**
     * Gets the metadata keys defined on the target object or its prototype chain.
     * @param target The target object on which the metadata is defined.
     * @param propertyKey (Optional) The property key for the target.
     * @returns An array of unique metadata keys.
     * @example
     *
     *     class Example {
     *         // property declarations are not part of ES6, though they are valid in TypeScript:
     *         // static staticProperty;
     *         // property;
     *
     *         constructor(p) { }
     *         static staticMethod(p) { }
     *         method(p) { }
     *     }
     *
     *     // constructor
     *     result = Reflect.getMetadataKeys(Example);
     *
     *     // property (on constructor)
     *     result = Reflect.getMetadataKeys(Example, "staticProperty");
     *
     *     // property (on prototype)
     *     result = Reflect.getMetadataKeys(Example.prototype, "property");
     *
     *     // method (on constructor)
     *     result = Reflect.getMetadataKeys(Example, "staticMethod");
     *
     *     // method (on prototype)
     *     result = Reflect.getMetadataKeys(Example.prototype, "method");
     *
     */

    function getMetadataKeys(target, propertyKey) {
      if (!IsObject(target)) throw new TypeError();
      if (!IsUndefined(propertyKey)) propertyKey = ToPropertyKey(propertyKey);
      return OrdinaryMetadataKeys(target, propertyKey);
    }

    exporter("getMetadataKeys", getMetadataKeys);
    /**
     * Gets the unique metadata keys defined on the target object.
     * @param target The target object on which the metadata is defined.
     * @param propertyKey (Optional) The property key for the target.
     * @returns An array of unique metadata keys.
     * @example
     *
     *     class Example {
     *         // property declarations are not part of ES6, though they are valid in TypeScript:
     *         // static staticProperty;
     *         // property;
     *
     *         constructor(p) { }
     *         static staticMethod(p) { }
     *         method(p) { }
     *     }
     *
     *     // constructor
     *     result = Reflect.getOwnMetadataKeys(Example);
     *
     *     // property (on constructor)
     *     result = Reflect.getOwnMetadataKeys(Example, "staticProperty");
     *
     *     // property (on prototype)
     *     result = Reflect.getOwnMetadataKeys(Example.prototype, "property");
     *
     *     // method (on constructor)
     *     result = Reflect.getOwnMetadataKeys(Example, "staticMethod");
     *
     *     // method (on prototype)
     *     result = Reflect.getOwnMetadataKeys(Example.prototype, "method");
     *
     */

    function getOwnMetadataKeys(target, propertyKey) {
      if (!IsObject(target)) throw new TypeError();
      if (!IsUndefined(propertyKey)) propertyKey = ToPropertyKey(propertyKey);
      return OrdinaryOwnMetadataKeys(target, propertyKey);
    }

    exporter("getOwnMetadataKeys", getOwnMetadataKeys);
    /**
     * Deletes the metadata entry from the target object with the provided key.
     * @param metadataKey A key used to store and retrieve metadata.
     * @param target The target object on which the metadata is defined.
     * @param propertyKey (Optional) The property key for the target.
     * @returns `true` if the metadata entry was found and deleted; otherwise, false.
     * @example
     *
     *     class Example {
     *         // property declarations are not part of ES6, though they are valid in TypeScript:
     *         // static staticProperty;
     *         // property;
     *
     *         constructor(p) { }
     *         static staticMethod(p) { }
     *         method(p) { }
     *     }
     *
     *     // constructor
     *     result = Reflect.deleteMetadata("custom:annotation", Example);
     *
     *     // property (on constructor)
     *     result = Reflect.deleteMetadata("custom:annotation", Example, "staticProperty");
     *
     *     // property (on prototype)
     *     result = Reflect.deleteMetadata("custom:annotation", Example.prototype, "property");
     *
     *     // method (on constructor)
     *     result = Reflect.deleteMetadata("custom:annotation", Example, "staticMethod");
     *
     *     // method (on prototype)
     *     result = Reflect.deleteMetadata("custom:annotation", Example.prototype, "method");
     *
     */

    function deleteMetadata(metadataKey, target, propertyKey) {
      if (!IsObject(target)) throw new TypeError();
      if (!IsUndefined(propertyKey)) propertyKey = ToPropertyKey(propertyKey);
      var metadataMap = GetOrCreateMetadataMap(target, propertyKey,
      /*Create*/
      false);
      if (IsUndefined(metadataMap)) return false;
      if (!metadataMap.delete(metadataKey)) return false;
      if (metadataMap.size > 0) return true;
      var targetMetadata = Metadata.get(target);
      targetMetadata.delete(propertyKey);
      if (targetMetadata.size > 0) return true;
      Metadata.delete(target);
      return true;
    }

    exporter("deleteMetadata", deleteMetadata);

    function DecorateConstructor(decorators, target) {
      for (var i = decorators.length - 1; i >= 0; --i) {
        var decorator = decorators[i];
        var decorated = decorator(target);

        if (!IsUndefined(decorated) && !IsNull(decorated)) {
          if (!IsConstructor(decorated)) throw new TypeError();
          target = decorated;
        }
      }

      return target;
    }

    function DecorateProperty(decorators, target, propertyKey, descriptor) {
      for (var i = decorators.length - 1; i >= 0; --i) {
        var decorator = decorators[i];
        var decorated = decorator(target, propertyKey, descriptor);

        if (!IsUndefined(decorated) && !IsNull(decorated)) {
          if (!IsObject(decorated)) throw new TypeError();
          descriptor = decorated;
        }
      }

      return descriptor;
    }

    function GetOrCreateMetadataMap(O, P, Create) {
      var targetMetadata = Metadata.get(O);

      if (IsUndefined(targetMetadata)) {
        if (!Create) return undefined;
        targetMetadata = new _Map();
        Metadata.set(O, targetMetadata);
      }

      var metadataMap = targetMetadata.get(P);

      if (IsUndefined(metadataMap)) {
        if (!Create) return undefined;
        metadataMap = new _Map();
        targetMetadata.set(P, metadataMap);
      }

      return metadataMap;
    } // 3.1.1.1 OrdinaryHasMetadata(MetadataKey, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinaryhasmetadata


    function OrdinaryHasMetadata(MetadataKey, O, P) {
      var hasOwn = OrdinaryHasOwnMetadata(MetadataKey, O, P);
      if (hasOwn) return true;
      var parent = OrdinaryGetPrototypeOf(O);
      if (!IsNull(parent)) return OrdinaryHasMetadata(MetadataKey, parent, P);
      return false;
    } // 3.1.2.1 OrdinaryHasOwnMetadata(MetadataKey, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinaryhasownmetadata


    function OrdinaryHasOwnMetadata(MetadataKey, O, P) {
      var metadataMap = GetOrCreateMetadataMap(O, P,
      /*Create*/
      false);
      if (IsUndefined(metadataMap)) return false;
      return ToBoolean(metadataMap.has(MetadataKey));
    } // 3.1.3.1 OrdinaryGetMetadata(MetadataKey, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinarygetmetadata


    function OrdinaryGetMetadata(MetadataKey, O, P) {
      var hasOwn = OrdinaryHasOwnMetadata(MetadataKey, O, P);
      if (hasOwn) return OrdinaryGetOwnMetadata(MetadataKey, O, P);
      var parent = OrdinaryGetPrototypeOf(O);
      if (!IsNull(parent)) return OrdinaryGetMetadata(MetadataKey, parent, P);
      return undefined;
    } // 3.1.4.1 OrdinaryGetOwnMetadata(MetadataKey, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinarygetownmetadata


    function OrdinaryGetOwnMetadata(MetadataKey, O, P) {
      var metadataMap = GetOrCreateMetadataMap(O, P,
      /*Create*/
      false);
      if (IsUndefined(metadataMap)) return undefined;
      return metadataMap.get(MetadataKey);
    } // 3.1.5.1 OrdinaryDefineOwnMetadata(MetadataKey, MetadataValue, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinarydefineownmetadata


    function OrdinaryDefineOwnMetadata(MetadataKey, MetadataValue, O, P) {
      var metadataMap = GetOrCreateMetadataMap(O, P,
      /*Create*/
      true);
      metadataMap.set(MetadataKey, MetadataValue);
    } // 3.1.6.1 OrdinaryMetadataKeys(O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinarymetadatakeys


    function OrdinaryMetadataKeys(O, P) {
      var ownKeys = OrdinaryOwnMetadataKeys(O, P);
      var parent = OrdinaryGetPrototypeOf(O);
      if (parent === null) return ownKeys;
      var parentKeys = OrdinaryMetadataKeys(parent, P);
      if (parentKeys.length <= 0) return ownKeys;
      if (ownKeys.length <= 0) return parentKeys;
      var set = new _Set();
      var keys = [];

      for (var _i = 0, ownKeys_1 = ownKeys; _i < ownKeys_1.length; _i++) {
        var key = ownKeys_1[_i];
        var hasKey = set.has(key);

        if (!hasKey) {
          set.add(key);
          keys.push(key);
        }
      }

      for (var _a = 0, parentKeys_1 = parentKeys; _a < parentKeys_1.length; _a++) {
        var key = parentKeys_1[_a];
        var hasKey = set.has(key);

        if (!hasKey) {
          set.add(key);
          keys.push(key);
        }
      }

      return keys;
    } // 3.1.7.1 OrdinaryOwnMetadataKeys(O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinaryownmetadatakeys


    function OrdinaryOwnMetadataKeys(O, P) {
      var keys = [];
      var metadataMap = GetOrCreateMetadataMap(O, P,
      /*Create*/
      false);
      if (IsUndefined(metadataMap)) return keys;
      var keysObj = metadataMap.keys();
      var iterator = GetIterator(keysObj);
      var k = 0;

      while (true) {
        var next = IteratorStep(iterator);

        if (!next) {
          keys.length = k;
          return keys;
        }

        var nextValue = IteratorValue(next);

        try {
          keys[k] = nextValue;
        } catch (e) {
          try {
            IteratorClose(iterator);
          } finally {
            throw e;
          }
        }

        k++;
      }
    } // 6 ECMAScript Data Typ0es and Values
    // https://tc39.github.io/ecma262/#sec-ecmascript-data-types-and-values


    function Type(x) {
      if (x === null) return 1
      /* Null */
      ;

      switch (typeof x) {
        case "undefined":
          return 0
          /* Undefined */
          ;

        case "boolean":
          return 2
          /* Boolean */
          ;

        case "string":
          return 3
          /* String */
          ;

        case "symbol":
          return 4
          /* Symbol */
          ;

        case "number":
          return 5
          /* Number */
          ;

        case "object":
          return x === null ? 1
          /* Null */
          : 6
          /* Object */
          ;

        default:
          return 6
          /* Object */
          ;
      }
    } // 6.1.1 The Undefined Type
    // https://tc39.github.io/ecma262/#sec-ecmascript-language-types-undefined-type


    function IsUndefined(x) {
      return x === undefined;
    } // 6.1.2 The Null Type
    // https://tc39.github.io/ecma262/#sec-ecmascript-language-types-null-type


    function IsNull(x) {
      return x === null;
    } // 6.1.5 The Symbol Type
    // https://tc39.github.io/ecma262/#sec-ecmascript-language-types-symbol-type


    function IsSymbol(x) {
      return typeof x === "symbol";
    } // 6.1.7 The Object Type
    // https://tc39.github.io/ecma262/#sec-object-type


    function IsObject(x) {
      return typeof x === "object" ? x !== null : typeof x === "function";
    } // 7.1 Type Conversion
    // https://tc39.github.io/ecma262/#sec-type-conversion
    // 7.1.1 ToPrimitive(input [, PreferredType])
    // https://tc39.github.io/ecma262/#sec-toprimitive


    function ToPrimitive(input, PreferredType) {
      switch (Type(input)) {
        case 0
        /* Undefined */
        :
          return input;

        case 1
        /* Null */
        :
          return input;

        case 2
        /* Boolean */
        :
          return input;

        case 3
        /* String */
        :
          return input;

        case 4
        /* Symbol */
        :
          return input;

        case 5
        /* Number */
        :
          return input;
      }

      var hint = PreferredType === 3
      /* String */
      ? "string" : PreferredType === 5
      /* Number */
      ? "number" : "default";
      var exoticToPrim = GetMethod(input, toPrimitiveSymbol);

      if (exoticToPrim !== undefined) {
        var result = exoticToPrim.call(input, hint);
        if (IsObject(result)) throw new TypeError();
        return result;
      }

      return OrdinaryToPrimitive(input, hint === "default" ? "number" : hint);
    } // 7.1.1.1 OrdinaryToPrimitive(O, hint)
    // https://tc39.github.io/ecma262/#sec-ordinarytoprimitive


    function OrdinaryToPrimitive(O, hint) {
      if (hint === "string") {
        var toString_1 = O.toString;

        if (IsCallable(toString_1)) {
          var result = toString_1.call(O);
          if (!IsObject(result)) return result;
        }

        var valueOf = O.valueOf;

        if (IsCallable(valueOf)) {
          var result = valueOf.call(O);
          if (!IsObject(result)) return result;
        }
      } else {
        var valueOf = O.valueOf;

        if (IsCallable(valueOf)) {
          var result = valueOf.call(O);
          if (!IsObject(result)) return result;
        }

        var toString_2 = O.toString;

        if (IsCallable(toString_2)) {
          var result = toString_2.call(O);
          if (!IsObject(result)) return result;
        }
      }

      throw new TypeError();
    } // 7.1.2 ToBoolean(argument)
    // https://tc39.github.io/ecma262/2016/#sec-toboolean


    function ToBoolean(argument) {
      return !!argument;
    } // 7.1.12 ToString(argument)
    // https://tc39.github.io/ecma262/#sec-tostring


    function ToString(argument) {
      return "" + argument;
    } // 7.1.14 ToPropertyKey(argument)
    // https://tc39.github.io/ecma262/#sec-topropertykey


    function ToPropertyKey(argument) {
      var key = ToPrimitive(argument, 3
      /* String */
      );
      if (IsSymbol(key)) return key;
      return ToString(key);
    } // 7.2 Testing and Comparison Operations
    // https://tc39.github.io/ecma262/#sec-testing-and-comparison-operations
    // 7.2.2 IsArray(argument)
    // https://tc39.github.io/ecma262/#sec-isarray


    function IsArray(argument) {
      return Array.isArray ? Array.isArray(argument) : argument instanceof Object ? argument instanceof Array : Object.prototype.toString.call(argument) === "[object Array]";
    } // 7.2.3 IsCallable(argument)
    // https://tc39.github.io/ecma262/#sec-iscallable


    function IsCallable(argument) {
      // NOTE: This is an approximation as we cannot check for [[Call]] internal method.
      return typeof argument === "function";
    } // 7.2.4 IsConstructor(argument)
    // https://tc39.github.io/ecma262/#sec-isconstructor


    function IsConstructor(argument) {
      // NOTE: This is an approximation as we cannot check for [[Construct]] internal method.
      return typeof argument === "function";
    } // 7.2.7 IsPropertyKey(argument)
    // https://tc39.github.io/ecma262/#sec-ispropertykey


    function IsPropertyKey(argument) {
      switch (Type(argument)) {
        case 3
        /* String */
        :
          return true;

        case 4
        /* Symbol */
        :
          return true;

        default:
          return false;
      }
    } // 7.3 Operations on Objects
    // https://tc39.github.io/ecma262/#sec-operations-on-objects
    // 7.3.9 GetMethod(V, P)
    // https://tc39.github.io/ecma262/#sec-getmethod


    function GetMethod(V, P) {
      var func = V[P];
      if (func === undefined || func === null) return undefined;
      if (!IsCallable(func)) throw new TypeError();
      return func;
    } // 7.4 Operations on Iterator Objects
    // https://tc39.github.io/ecma262/#sec-operations-on-iterator-objects


    function GetIterator(obj) {
      var method = GetMethod(obj, iteratorSymbol);
      if (!IsCallable(method)) throw new TypeError(); // from Call

      var iterator = method.call(obj);
      if (!IsObject(iterator)) throw new TypeError();
      return iterator;
    } // 7.4.4 IteratorValue(iterResult)
    // https://tc39.github.io/ecma262/2016/#sec-iteratorvalue


    function IteratorValue(iterResult) {
      return iterResult.value;
    } // 7.4.5 IteratorStep(iterator)
    // https://tc39.github.io/ecma262/#sec-iteratorstep


    function IteratorStep(iterator) {
      var result = iterator.next();
      return result.done ? false : result;
    } // 7.4.6 IteratorClose(iterator, completion)
    // https://tc39.github.io/ecma262/#sec-iteratorclose


    function IteratorClose(iterator) {
      var f = iterator["return"];
      if (f) f.call(iterator);
    } // 9.1 Ordinary Object Internal Methods and Internal Slots
    // https://tc39.github.io/ecma262/#sec-ordinary-object-internal-methods-and-internal-slots
    // 9.1.1.1 OrdinaryGetPrototypeOf(O)
    // https://tc39.github.io/ecma262/#sec-ordinarygetprototypeof


    function OrdinaryGetPrototypeOf(O) {
      var proto = Object.getPrototypeOf(O);
      if (typeof O !== "function" || O === functionPrototype) return proto; // TypeScript doesn't set __proto__ in ES5, as it's non-standard.
      // Try to determine the superclass constructor. Compatible implementations
      // must either set __proto__ on a subclass constructor to the superclass constructor,
      // or ensure each class has a valid `constructor` property on its prototype that
      // points back to the constructor.
      // If this is not the same as Function.[[Prototype]], then this is definately inherited.
      // This is the case when in ES6 or when using __proto__ in a compatible browser.

      if (proto !== functionPrototype) return proto; // If the super prototype is Object.prototype, null, or undefined, then we cannot determine the heritage.

      var prototype = O.prototype;
      var prototypeProto = prototype && Object.getPrototypeOf(prototype);
      if (prototypeProto == null || prototypeProto === Object.prototype) return proto; // If the constructor was not a function, then we cannot determine the heritage.

      var constructor = prototypeProto.constructor;
      if (typeof constructor !== "function") return proto; // If we have some kind of self-reference, then we cannot determine the heritage.

      if (constructor === O) return proto; // we have a pretty good guess at the heritage.

      return constructor;
    } // naive Map shim


    function CreateMapPolyfill() {
      var cacheSentinel = {};
      var arraySentinel = [];

      var MapIterator =
      /** @class */
      function () {
        function MapIterator(keys, values, selector) {
          this._index = 0;
          this._keys = keys;
          this._values = values;
          this._selector = selector;
        }

        MapIterator.prototype["@@iterator"] = function () {
          return this;
        };

        MapIterator.prototype[iteratorSymbol] = function () {
          return this;
        };

        MapIterator.prototype.next = function () {
          var index = this._index;

          if (index >= 0 && index < this._keys.length) {
            var result = this._selector(this._keys[index], this._values[index]);

            if (index + 1 >= this._keys.length) {
              this._index = -1;
              this._keys = arraySentinel;
              this._values = arraySentinel;
            } else {
              this._index++;
            }

            return {
              value: result,
              done: false
            };
          }

          return {
            value: undefined,
            done: true
          };
        };

        MapIterator.prototype.throw = function (error) {
          if (this._index >= 0) {
            this._index = -1;
            this._keys = arraySentinel;
            this._values = arraySentinel;
          }

          throw error;
        };

        MapIterator.prototype.return = function (value) {
          if (this._index >= 0) {
            this._index = -1;
            this._keys = arraySentinel;
            this._values = arraySentinel;
          }

          return {
            value: value,
            done: true
          };
        };

        return MapIterator;
      }();

      return (
        /** @class */
        function () {
          function Map() {
            this._keys = [];
            this._values = [];
            this._cacheKey = cacheSentinel;
            this._cacheIndex = -2;
          }

          Object.defineProperty(Map.prototype, "size", {
            get: function () {
              return this._keys.length;
            },
            enumerable: true,
            configurable: true
          });

          Map.prototype.has = function (key) {
            return this._find(key,
            /*insert*/
            false) >= 0;
          };

          Map.prototype.get = function (key) {
            var index = this._find(key,
            /*insert*/
            false);

            return index >= 0 ? this._values[index] : undefined;
          };

          Map.prototype.set = function (key, value) {
            var index = this._find(key,
            /*insert*/
            true);

            this._values[index] = value;
            return this;
          };

          Map.prototype.delete = function (key) {
            var index = this._find(key,
            /*insert*/
            false);

            if (index >= 0) {
              var size = this._keys.length;

              for (var i = index + 1; i < size; i++) {
                this._keys[i - 1] = this._keys[i];
                this._values[i - 1] = this._values[i];
              }

              this._keys.length--;
              this._values.length--;

              if (key === this._cacheKey) {
                this._cacheKey = cacheSentinel;
                this._cacheIndex = -2;
              }

              return true;
            }

            return false;
          };

          Map.prototype.clear = function () {
            this._keys.length = 0;
            this._values.length = 0;
            this._cacheKey = cacheSentinel;
            this._cacheIndex = -2;
          };

          Map.prototype.keys = function () {
            return new MapIterator(this._keys, this._values, getKey);
          };

          Map.prototype.values = function () {
            return new MapIterator(this._keys, this._values, getValue);
          };

          Map.prototype.entries = function () {
            return new MapIterator(this._keys, this._values, getEntry);
          };

          Map.prototype["@@iterator"] = function () {
            return this.entries();
          };

          Map.prototype[iteratorSymbol] = function () {
            return this.entries();
          };

          Map.prototype._find = function (key, insert) {
            if (this._cacheKey !== key) {
              this._cacheIndex = this._keys.indexOf(this._cacheKey = key);
            }

            if (this._cacheIndex < 0 && insert) {
              this._cacheIndex = this._keys.length;

              this._keys.push(key);

              this._values.push(undefined);
            }

            return this._cacheIndex;
          };

          return Map;
        }()
      );

      function getKey(key, _) {
        return key;
      }

      function getValue(_, value) {
        return value;
      }

      function getEntry(key, value) {
        return [key, value];
      }
    } // naive Set shim


    function CreateSetPolyfill() {
      return (
        /** @class */
        function () {
          function Set() {
            this._map = new _Map();
          }

          Object.defineProperty(Set.prototype, "size", {
            get: function () {
              return this._map.size;
            },
            enumerable: true,
            configurable: true
          });

          Set.prototype.has = function (value) {
            return this._map.has(value);
          };

          Set.prototype.add = function (value) {
            return this._map.set(value, value), this;
          };

          Set.prototype.delete = function (value) {
            return this._map.delete(value);
          };

          Set.prototype.clear = function () {
            this._map.clear();
          };

          Set.prototype.keys = function () {
            return this._map.keys();
          };

          Set.prototype.values = function () {
            return this._map.values();
          };

          Set.prototype.entries = function () {
            return this._map.entries();
          };

          Set.prototype["@@iterator"] = function () {
            return this.keys();
          };

          Set.prototype[iteratorSymbol] = function () {
            return this.keys();
          };

          return Set;
        }()
      );
    } // naive WeakMap shim


    function CreateWeakMapPolyfill() {
      var UUID_SIZE = 16;
      var keys = HashMap.create();
      var rootKey = CreateUniqueKey();
      return (
        /** @class */
        function () {
          function WeakMap() {
            this._key = CreateUniqueKey();
          }

          WeakMap.prototype.has = function (target) {
            var table = GetOrCreateWeakMapTable(target,
            /*create*/
            false);
            return table !== undefined ? HashMap.has(table, this._key) : false;
          };

          WeakMap.prototype.get = function (target) {
            var table = GetOrCreateWeakMapTable(target,
            /*create*/
            false);
            return table !== undefined ? HashMap.get(table, this._key) : undefined;
          };

          WeakMap.prototype.set = function (target, value) {
            var table = GetOrCreateWeakMapTable(target,
            /*create*/
            true);
            table[this._key] = value;
            return this;
          };

          WeakMap.prototype.delete = function (target) {
            var table = GetOrCreateWeakMapTable(target,
            /*create*/
            false);
            return table !== undefined ? delete table[this._key] : false;
          };

          WeakMap.prototype.clear = function () {
            // NOTE: not a real clear, just makes the previous data unreachable
            this._key = CreateUniqueKey();
          };

          return WeakMap;
        }()
      );

      function CreateUniqueKey() {
        var key;

        do key = "@@WeakMap@@" + CreateUUID(); while (HashMap.has(keys, key));

        keys[key] = true;
        return key;
      }

      function GetOrCreateWeakMapTable(target, create) {
        if (!hasOwn.call(target, rootKey)) {
          if (!create) return undefined;
          Object.defineProperty(target, rootKey, {
            value: HashMap.create()
          });
        }

        return target[rootKey];
      }

      function FillRandomBytes(buffer, size) {
        for (var i = 0; i < size; ++i) buffer[i] = Math.random() * 0xff | 0;

        return buffer;
      }

      function GenRandomBytes(size) {
        if (typeof Uint8Array === "function") {
          if (typeof crypto !== "undefined") return crypto.getRandomValues(new Uint8Array(size));
          if (typeof msCrypto !== "undefined") return msCrypto.getRandomValues(new Uint8Array(size));
          return FillRandomBytes(new Uint8Array(size), size);
        }

        return FillRandomBytes(new Array(size), size);
      }

      function CreateUUID() {
        var data = GenRandomBytes(UUID_SIZE); // mark as random - RFC 4122 § 4.4

        data[6] = data[6] & 0x4f | 0x40;
        data[8] = data[8] & 0xbf | 0x80;
        var result = "";

        for (var offset = 0; offset < UUID_SIZE; ++offset) {
          var byte = data[offset];
          if (offset === 4 || offset === 6 || offset === 8) result += "-";
          if (byte < 16) result += "0";
          result += byte.toString(16).toLowerCase();
        }

        return result;
      }
    } // uses a heuristic used by v8 and chakra to force an object into dictionary mode.


    function MakeDictionary(obj) {
      obj.__ = undefined;
      delete obj.__;
      return obj;
    }
  });
})(Reflect$1 || (Reflect$1 = {}));

var IocManager =
/*#__PURE__*/
function () {
  function IocManager() {
    _classCallCheck(this, IocManager);

    this.denpendcyRegisters = [];
    this.diConainter = new inversify_2();
  }

  _createClass(IocManager, [{
    key: "addDenpendcyRegistrar",
    value: function addDenpendcyRegistrar(registrar) {
      this.denpendcyRegisters.push(registrar);
    }
  }, {
    key: "registerModuleByConvention",
    value: function registerModuleByConvention(moduleContexts) {
      var registerCTX = new RegistrationContext(moduleContexts, this);
      var _iteratorNormalCompletion = true;
      var _didIteratorError = false;
      var _iteratorError = undefined;

      try {
        for (var _iterator = getIterator$1(this.denpendcyRegisters), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
          var register = _step.value;
          register.register(registerCTX);
        }
      } catch (err) {
        _didIteratorError = true;
        _iteratorError = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion && _iterator.return != null) {
            _iterator.return();
          }
        } finally {
          if (_didIteratorError) {
            throw _iteratorError;
          }
        }
      }
    }
  }, {
    key: "register",
    value: function register(ContainerDelegate) {
      if (ContainerDelegate !== undefined) ContainerDelegate(this.diConainter);
    }
  }, {
    key: "get",
    value: function get(serviceIdentifier) {
      return this.diConainter.get(serviceIdentifier);
    }
  }, {
    key: "getAll",
    value: function getAll(serviceIdentifier) {
      return this.diConainter.getAll(serviceIdentifier);
    }
  }, {
    key: "isRegistered",
    value: function isRegistered(serviceIdentifier) {
      return this.diConainter.isBound(serviceIdentifier);
    }
  }, {
    key: "unbindAll",
    value: function unbindAll() {
      this.diConainter.unbindAll();
    }
  }]);

  return IocManager;
}();

var RegistrationContext = function RegistrationContext(moduleContexts, iocManager) {
  _classCallCheck(this, RegistrationContext);

  this.iocManger = iocManager;
  this.moduleContexts = moduleContexts;
};

var BootstrapperOptions = function BootstrapperOptions() {
  _classCallCheck(this, BootstrapperOptions);

  this.iocManager = new IocManager();
};

var createProperty = function (object, key, value) {
  var propertyKey = toPrimitive(key);
  if (propertyKey in object) objectDefineProperty.f(object, propertyKey, createPropertyDescriptor(0, value));else object[propertyKey] = value;
};

var userAgent = getBuiltIn('navigator', 'userAgent') || '';

var process$1 = global_1.process;
var versions = process$1 && process$1.versions;
var v8 = versions && versions.v8;
var match, version;

if (v8) {
  match = v8.split('.');
  version = match[0] + match[1];
} else if (userAgent) {
  match = userAgent.match(/Edge\/(\d+)/);

  if (!match || match[1] >= 74) {
    match = userAgent.match(/Chrome\/(\d+)/);
    if (match) version = match[1];
  }
}

var v8Version = version && +version;

var SPECIES$1 = wellKnownSymbol('species');

var arrayMethodHasSpeciesSupport = function (METHOD_NAME) {
  // We can't use this feature detection in V8 since it causes
  // deoptimization and serious performance degradation
  // https://github.com/zloirock/core-js/issues/677
  return v8Version >= 51 || !fails(function () {
    var array = [];
    var constructor = array.constructor = {};

    constructor[SPECIES$1] = function () {
      return {
        foo: 1
      };
    };

    return array[METHOD_NAME](Boolean).foo !== 1;
  });
};

var IS_CONCAT_SPREADABLE = wellKnownSymbol('isConcatSpreadable');
var MAX_SAFE_INTEGER = 0x1FFFFFFFFFFFFF;
var MAXIMUM_ALLOWED_INDEX_EXCEEDED = 'Maximum allowed index exceeded'; // We can't use this feature detection in V8 since it causes
// deoptimization and serious performance degradation
// https://github.com/zloirock/core-js/issues/679

var IS_CONCAT_SPREADABLE_SUPPORT = v8Version >= 51 || !fails(function () {
  var array = [];
  array[IS_CONCAT_SPREADABLE] = false;
  return array.concat()[0] !== array;
});
var SPECIES_SUPPORT = arrayMethodHasSpeciesSupport('concat');

var isConcatSpreadable = function (O) {
  if (!isObject(O)) return false;
  var spreadable = O[IS_CONCAT_SPREADABLE];
  return spreadable !== undefined ? !!spreadable : isArray(O);
};

var FORCED = !IS_CONCAT_SPREADABLE_SUPPORT || !SPECIES_SUPPORT; // `Array.prototype.concat` method
// https://tc39.github.io/ecma262/#sec-array.prototype.concat
// with adding support of @@isConcatSpreadable and @@species

_export({
  target: 'Array',
  proto: true,
  forced: FORCED
}, {
  concat: function concat(arg) {
    // eslint-disable-line no-unused-vars
    var O = toObject(this);
    var A = arraySpeciesCreate(O, 0);
    var n = 0;
    var i, k, length, len, E;

    for (i = -1, length = arguments.length; i < length; i++) {
      E = i === -1 ? O : arguments[i];

      if (isConcatSpreadable(E)) {
        len = toLength(E.length);
        if (n + len > MAX_SAFE_INTEGER) throw TypeError(MAXIMUM_ALLOWED_INDEX_EXCEEDED);

        for (k = 0; k < len; k++, n++) if (k in E) createProperty(A, n, E[k]);
      } else {
        if (n >= MAX_SAFE_INTEGER) throw TypeError(MAXIMUM_ALLOWED_INDEX_EXCEEDED);
        createProperty(A, n++, E);
      }
    }

    A.length = n;
    return A;
  }
});

var concat = entryVirtual('Array').concat;

var ArrayPrototype = Array.prototype;

var concat_1 = function (it) {
  var own = it.concat;
  return it === ArrayPrototype || it instanceof Array && own === ArrayPrototype.concat ? concat : own;
};

var concat$1 = concat_1;

var concat$2 = concat$1;

var SPECIES$2 = wellKnownSymbol('species');
var nativeSlice = [].slice;
var max$1 = Math.max; // `Array.prototype.slice` method
// https://tc39.github.io/ecma262/#sec-array.prototype.slice
// fallback for not array-like ES3 strings and DOM objects

_export({
  target: 'Array',
  proto: true,
  forced: !arrayMethodHasSpeciesSupport('slice')
}, {
  slice: function slice(start, end) {
    var O = toIndexedObject(this);
    var length = toLength(O.length);
    var k = toAbsoluteIndex(start, length);
    var fin = toAbsoluteIndex(end === undefined ? length : end, length); // inline `ArraySpeciesCreate` for usage native `Array#slice` where it's possible

    var Constructor, result, n;

    if (isArray(O)) {
      Constructor = O.constructor; // cross-realm fallback

      if (typeof Constructor == 'function' && (Constructor === Array || isArray(Constructor.prototype))) {
        Constructor = undefined;
      } else if (isObject(Constructor)) {
        Constructor = Constructor[SPECIES$2];
        if (Constructor === null) Constructor = undefined;
      }

      if (Constructor === Array || Constructor === undefined) {
        return nativeSlice.call(O, k, fin);
      }
    }

    result = new (Constructor === undefined ? Array : Constructor)(max$1(fin - k, 0));

    for (n = 0; k < fin; k++, n++) if (k in O) createProperty(result, n, O[k]);

    result.length = n;
    return result;
  }
});

var slice$1 = entryVirtual('Array').slice;

var ArrayPrototype$1 = Array.prototype;

var slice_1 = function (it) {
  var own = it.slice;
  return it === ArrayPrototype$1 || it instanceof Array && own === ArrayPrototype$1.slice ? slice$1 : own;
};

var slice$2 = slice_1;

var slice$3 = slice$2;

var Types = {
  IBlocksShell: _for$2("IBlocksShell"),
  IRouteProvider: _for$2("IRouteProvider"),
  ITemplateProvider: _for$2("ITemplateProvider"),
  IBootstrapper: _for$2("IBootstrapper"),
  IManifestProvider: _for$2("IManifestProvider"),
  IResourceManager: _for$2("IResourceManager"),
  IRouteManager: _for$2("IRouteManager")
};

var freezing = !fails(function () {
  return Object.isExtensible(Object.preventExtensions({}));
});

var internalMetadata = createCommonjsModule(function (module) {
  var defineProperty = objectDefineProperty.f;
  var METADATA = uid('meta');
  var id = 0;

  var isExtensible = Object.isExtensible || function () {
    return true;
  };

  var setMetadata = function (it) {
    defineProperty(it, METADATA, {
      value: {
        objectID: 'O' + ++id,
        // object ID
        weakData: {} // weak collections IDs

      }
    });
  };

  var fastKey = function (it, create) {
    // return a primitive with prefix
    if (!isObject(it)) return typeof it == 'symbol' ? it : (typeof it == 'string' ? 'S' : 'P') + it;

    if (!has(it, METADATA)) {
      // can't set metadata to uncaught frozen object
      if (!isExtensible(it)) return 'F'; // not necessary to add metadata

      if (!create) return 'E'; // add missing metadata

      setMetadata(it); // return object ID
    }

    return it[METADATA].objectID;
  };

  var getWeakData = function (it, create) {
    if (!has(it, METADATA)) {
      // can't set metadata to uncaught frozen object
      if (!isExtensible(it)) return true; // not necessary to add metadata

      if (!create) return false; // add missing metadata

      setMetadata(it); // return the store of weak collections IDs
    }

    return it[METADATA].weakData;
  }; // add metadata on freeze-family methods calling


  var onFreeze = function (it) {
    if (freezing && meta.REQUIRED && isExtensible(it) && !has(it, METADATA)) setMetadata(it);
    return it;
  };

  var meta = module.exports = {
    REQUIRED: false,
    fastKey: fastKey,
    getWeakData: getWeakData,
    onFreeze: onFreeze
  };
  hiddenKeys[METADATA] = true;
});
var internalMetadata_1 = internalMetadata.REQUIRED;
var internalMetadata_2 = internalMetadata.fastKey;
var internalMetadata_3 = internalMetadata.getWeakData;
var internalMetadata_4 = internalMetadata.onFreeze;

var ITERATOR$3 = wellKnownSymbol('iterator');
var ArrayPrototype$2 = Array.prototype; // check on default Array iterator

var isArrayIteratorMethod = function (it) {
  return it !== undefined && (iterators.Array === it || ArrayPrototype$2[ITERATOR$3] === it);
};

var callWithSafeIterationClosing = function (iterator, fn, value, ENTRIES) {
  try {
    return ENTRIES ? fn(anObject(value)[0], value[1]) : fn(value); // 7.4.6 IteratorClose(iterator, completion)
  } catch (error) {
    var returnMethod = iterator['return'];
    if (returnMethod !== undefined) anObject(returnMethod.call(iterator));
    throw error;
  }
};

var iterate_1 = createCommonjsModule(function (module) {
  var Result = function (stopped, result) {
    this.stopped = stopped;
    this.result = result;
  };

  var iterate = module.exports = function (iterable, fn, that, AS_ENTRIES, IS_ITERATOR) {
    var boundFunction = bindContext(fn, that, AS_ENTRIES ? 2 : 1);
    var iterator, iterFn, index, length, result, next, step;

    if (IS_ITERATOR) {
      iterator = iterable;
    } else {
      iterFn = getIteratorMethod(iterable);
      if (typeof iterFn != 'function') throw TypeError('Target is not iterable'); // optimisation for array iterators

      if (isArrayIteratorMethod(iterFn)) {
        for (index = 0, length = toLength(iterable.length); length > index; index++) {
          result = AS_ENTRIES ? boundFunction(anObject(step = iterable[index])[0], step[1]) : boundFunction(iterable[index]);
          if (result && result instanceof Result) return result;
        }

        return new Result(false);
      }

      iterator = iterFn.call(iterable);
    }

    next = iterator.next;

    while (!(step = next.call(iterator)).done) {
      result = callWithSafeIterationClosing(iterator, boundFunction, step.value, AS_ENTRIES);
      if (typeof result == 'object' && result && result instanceof Result) return result;
    }

    return new Result(false);
  };

  iterate.stop = function (result) {
    return new Result(true, result);
  };
});

var anInstance = function (it, Constructor, name) {
  if (!(it instanceof Constructor)) {
    throw TypeError('Incorrect ' + (name ? name + ' ' : '') + 'invocation');
  }

  return it;
};

var defineProperty$2 = objectDefineProperty.f;
var forEach = arrayIteration.forEach;
var setInternalState$3 = internalState.set;
var internalStateGetterFor = internalState.getterFor;

var collection = function (CONSTRUCTOR_NAME, wrapper, common) {
  var IS_MAP = CONSTRUCTOR_NAME.indexOf('Map') !== -1;
  var IS_WEAK = CONSTRUCTOR_NAME.indexOf('Weak') !== -1;
  var ADDER = IS_MAP ? 'set' : 'add';
  var NativeConstructor = global_1[CONSTRUCTOR_NAME];
  var NativePrototype = NativeConstructor && NativeConstructor.prototype;
  var exported = {};
  var Constructor;

  if (!descriptors || typeof NativeConstructor != 'function' || !(IS_WEAK || NativePrototype.forEach && !fails(function () {
    new NativeConstructor().entries().next();
  }))) {
    // create collection constructor
    Constructor = common.getConstructor(wrapper, CONSTRUCTOR_NAME, IS_MAP, ADDER);
    internalMetadata.REQUIRED = true;
  } else {
    Constructor = wrapper(function (target, iterable) {
      setInternalState$3(anInstance(target, Constructor, CONSTRUCTOR_NAME), {
        type: CONSTRUCTOR_NAME,
        collection: new NativeConstructor()
      });
      if (iterable != undefined) iterate_1(iterable, target[ADDER], target, IS_MAP);
    });
    var getInternalState = internalStateGetterFor(CONSTRUCTOR_NAME);
    forEach(['add', 'clear', 'delete', 'forEach', 'get', 'has', 'set', 'keys', 'values', 'entries'], function (KEY) {
      var IS_ADDER = KEY == 'add' || KEY == 'set';

      if (KEY in NativePrototype && !(IS_WEAK && KEY == 'clear')) {
        createNonEnumerableProperty(Constructor.prototype, KEY, function (a, b) {
          var collection = getInternalState(this).collection;
          if (!IS_ADDER && IS_WEAK && !isObject(a)) return KEY == 'get' ? undefined : false;
          var result = collection[KEY](a === 0 ? 0 : a, b);
          return IS_ADDER ? this : result;
        });
      }
    });
    IS_WEAK || defineProperty$2(Constructor.prototype, 'size', {
      configurable: true,
      get: function () {
        return getInternalState(this).collection.size;
      }
    });
  }

  setToStringTag(Constructor, CONSTRUCTOR_NAME, false, true);
  exported[CONSTRUCTOR_NAME] = Constructor;
  _export({
    global: true,
    forced: true
  }, exported);
  if (!IS_WEAK) common.setStrong(Constructor, CONSTRUCTOR_NAME, IS_MAP);
  return Constructor;
};

var redefineAll = function (target, src, options) {
  for (var key in src) {
    if (options && options.unsafe && target[key]) target[key] = src[key];else redefine(target, key, src[key], options);
  }

  return target;
};

var SPECIES$3 = wellKnownSymbol('species');

var setSpecies = function (CONSTRUCTOR_NAME) {
  var Constructor = getBuiltIn(CONSTRUCTOR_NAME);
  var defineProperty = objectDefineProperty.f;

  if (descriptors && Constructor && !Constructor[SPECIES$3]) {
    defineProperty(Constructor, SPECIES$3, {
      configurable: true,
      get: function () {
        return this;
      }
    });
  }
};

var defineProperty$3 = objectDefineProperty.f;
var fastKey = internalMetadata.fastKey;
var setInternalState$4 = internalState.set;
var internalStateGetterFor$1 = internalState.getterFor;
var collectionStrong = {
  getConstructor: function (wrapper, CONSTRUCTOR_NAME, IS_MAP, ADDER) {
    var C = wrapper(function (that, iterable) {
      anInstance(that, C, CONSTRUCTOR_NAME);
      setInternalState$4(that, {
        type: CONSTRUCTOR_NAME,
        index: objectCreate(null),
        first: undefined,
        last: undefined,
        size: 0
      });
      if (!descriptors) that.size = 0;
      if (iterable != undefined) iterate_1(iterable, that[ADDER], that, IS_MAP);
    });
    var getInternalState = internalStateGetterFor$1(CONSTRUCTOR_NAME);

    var define = function (that, key, value) {
      var state = getInternalState(that);
      var entry = getEntry(that, key);
      var previous, index; // change existing entry

      if (entry) {
        entry.value = value; // create new entry
      } else {
        state.last = entry = {
          index: index = fastKey(key, true),
          key: key,
          value: value,
          previous: previous = state.last,
          next: undefined,
          removed: false
        };
        if (!state.first) state.first = entry;
        if (previous) previous.next = entry;
        if (descriptors) state.size++;else that.size++; // add to index

        if (index !== 'F') state.index[index] = entry;
      }

      return that;
    };

    var getEntry = function (that, key) {
      var state = getInternalState(that); // fast case

      var index = fastKey(key);
      var entry;
      if (index !== 'F') return state.index[index]; // frozen object case

      for (entry = state.first; entry; entry = entry.next) {
        if (entry.key == key) return entry;
      }
    };

    redefineAll(C.prototype, {
      // 23.1.3.1 Map.prototype.clear()
      // 23.2.3.2 Set.prototype.clear()
      clear: function clear() {
        var that = this;
        var state = getInternalState(that);
        var data = state.index;
        var entry = state.first;

        while (entry) {
          entry.removed = true;
          if (entry.previous) entry.previous = entry.previous.next = undefined;
          delete data[entry.index];
          entry = entry.next;
        }

        state.first = state.last = undefined;
        if (descriptors) state.size = 0;else that.size = 0;
      },
      // 23.1.3.3 Map.prototype.delete(key)
      // 23.2.3.4 Set.prototype.delete(value)
      'delete': function (key) {
        var that = this;
        var state = getInternalState(that);
        var entry = getEntry(that, key);

        if (entry) {
          var next = entry.next;
          var prev = entry.previous;
          delete state.index[entry.index];
          entry.removed = true;
          if (prev) prev.next = next;
          if (next) next.previous = prev;
          if (state.first == entry) state.first = next;
          if (state.last == entry) state.last = prev;
          if (descriptors) state.size--;else that.size--;
        }

        return !!entry;
      },
      // 23.2.3.6 Set.prototype.forEach(callbackfn, thisArg = undefined)
      // 23.1.3.5 Map.prototype.forEach(callbackfn, thisArg = undefined)
      forEach: function forEach(callbackfn
      /* , that = undefined */
      ) {
        var state = getInternalState(this);
        var boundFunction = bindContext(callbackfn, arguments.length > 1 ? arguments[1] : undefined, 3);
        var entry;

        while (entry = entry ? entry.next : state.first) {
          boundFunction(entry.value, entry.key, this); // revert to the last existing entry

          while (entry && entry.removed) entry = entry.previous;
        }
      },
      // 23.1.3.7 Map.prototype.has(key)
      // 23.2.3.7 Set.prototype.has(value)
      has: function has(key) {
        return !!getEntry(this, key);
      }
    });
    redefineAll(C.prototype, IS_MAP ? {
      // 23.1.3.6 Map.prototype.get(key)
      get: function get(key) {
        var entry = getEntry(this, key);
        return entry && entry.value;
      },
      // 23.1.3.9 Map.prototype.set(key, value)
      set: function set(key, value) {
        return define(this, key === 0 ? 0 : key, value);
      }
    } : {
      // 23.2.3.1 Set.prototype.add(value)
      add: function add(value) {
        return define(this, value = value === 0 ? 0 : value, value);
      }
    });
    if (descriptors) defineProperty$3(C.prototype, 'size', {
      get: function () {
        return getInternalState(this).size;
      }
    });
    return C;
  },
  setStrong: function (C, CONSTRUCTOR_NAME, IS_MAP) {
    var ITERATOR_NAME = CONSTRUCTOR_NAME + ' Iterator';
    var getInternalCollectionState = internalStateGetterFor$1(CONSTRUCTOR_NAME);
    var getInternalIteratorState = internalStateGetterFor$1(ITERATOR_NAME); // add .keys, .values, .entries, [@@iterator]
    // 23.1.3.4, 23.1.3.8, 23.1.3.11, 23.1.3.12, 23.2.3.5, 23.2.3.8, 23.2.3.10, 23.2.3.11

    defineIterator(C, CONSTRUCTOR_NAME, function (iterated, kind) {
      setInternalState$4(this, {
        type: ITERATOR_NAME,
        target: iterated,
        state: getInternalCollectionState(iterated),
        kind: kind,
        last: undefined
      });
    }, function () {
      var state = getInternalIteratorState(this);
      var kind = state.kind;
      var entry = state.last; // revert to the last existing entry

      while (entry && entry.removed) entry = entry.previous; // get next entry


      if (!state.target || !(state.last = entry = entry ? entry.next : state.state.first)) {
        // or finish the iteration
        state.target = undefined;
        return {
          value: undefined,
          done: true
        };
      } // return step by kind


      if (kind == 'keys') return {
        value: entry.key,
        done: false
      };
      if (kind == 'values') return {
        value: entry.value,
        done: false
      };
      return {
        value: [entry.key, entry.value],
        done: false
      };
    }, IS_MAP ? 'entries' : 'values', !IS_MAP, true); // add [@@species], 23.1.2.2, 23.2.2.2

    setSpecies(CONSTRUCTOR_NAME);
  }
};

// https://tc39.github.io/ecma262/#sec-map-objects


var es_map = collection('Map', function (init) {
  return function Map() {
    return init(this, arguments.length ? arguments[0] : undefined);
  };
}, collectionStrong);

var map = path.Map;

var map$1 = map;

var map$2 = map$1;

/*! *****************************************************************************
Copyright (c) Microsoft Corporation. All rights reserved.
Licensed under the Apache License, Version 2.0 (the "License"); you may not use
this file except in compliance with the License. You may obtain a copy of the
License at http://www.apache.org/licenses/LICENSE-2.0

THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
MERCHANTABLITY OR NON-INFRINGEMENT.

See the Apache Version 2.0 License for specific language governing permissions
and limitations under the License.
***************************************************************************** */

function __decorate(decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
}

function __param(paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
}

function __metadata(metadataKey, metadataValue) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(metadataKey, metadataValue);
}

var _win = window;
var globalIocManager = _win["globalIocManager"] || new IocManager();
_win["globalIocManager"] = globalIocManager;

var IDependency = function IDependency() {
  _classCallCheck(this, IDependency);
};

IDependency = __decorate([inversify_8()], IDependency);

var IBlocksShell =
/*#__PURE__*/
function () {
  function IBlocksShell() {
    _classCallCheck(this, IBlocksShell);

    this.types = [];
    this.typeMapModuleName = new map$2();
    this.moduleMapTypes = new map$2();
  }

  _createClass(IBlocksShell, [{
    key: "initialize",
    value: function initialize() {
      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function initialize$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              throw new Error("initialize is not implemented. ");

            case 1:
            case "end":
              return _context.stop();
          }
        }
      });
    }
  }, {
    key: "BlocksModules",
    get: function get() {
      throw new Error("initialize is not implemented. ");
    }
  }]);

  return IBlocksShell;
}();

var IBootstrapper =
/*#__PURE__*/
function () {
  function IBootstrapper() {
    _classCallCheck(this, IBootstrapper);
  }

  _createClass(IBootstrapper, [{
    key: "initialize",
    value: function initialize() {
      throw new Error("initialize is not implemented. ");
    }
  }, {
    key: "dispose",
    value: function dispose() {
      throw new Error("dispose is not implemented. ");
    }
  }, {
    key: "PlugInSources",
    get: function get() {
      throw new Error("plugInSources is not implemented. ");
    }
  }]);

  return IBootstrapper;
}();

var IShell =
/*#__PURE__*/
function (_IDependency2) {
  _inherits(IShell, _IDependency2);

  function IShell() {
    var _this2;

    _classCallCheck(this, IShell);

    _this2 = _possibleConstructorReturn(this, _getPrototypeOf(IShell).apply(this, arguments));
    _this2.pluginSource = [];
    _this2.types = [];
    _this2.moduleMapTypes = new map$2();
    _this2.typeMapModuleName = new map$2();
    return _this2;
  }

  _createClass(IShell, [{
    key: "initialize",
    value: function initialize() {}
  }]);

  return IShell;
}(IDependency);

var BlocksModule =
/*#__PURE__*/
function (_IDependency3) {
  _inherits(BlocksModule, _IDependency3);

  function BlocksModule() {
    var _this3;

    _classCallCheck(this, BlocksModule);

    _this3 = _possibleConstructorReturn(this, _getPrototypeOf(BlocksModule).call(this)); //TODO Need to SortModule??

    _this3.order = 10;

    _this3.providers = function () {
      return [];
    };

    _this3.moduleName = "default";
    return _this3;
  }

  _createClass(BlocksModule, [{
    key: "preInitialize",
    value: function preInitialize() {
      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function preInitialize$(_context2) {
        while (1) {
          switch (_context2.prev = _context2.next) {
            case 0:
            case "end":
              return _context2.stop();
          }
        }
      });
    }
  }, {
    key: "initialize",
    value: function initialize() {
      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function initialize$(_context3) {
        while (1) {
          switch (_context3.prev = _context3.next) {
            case 0:
            case "end":
              return _context3.stop();
          }
        }
      });
    }
  }, {
    key: "getProviders",
    value: function getProviders() {
      return this.providers();
    }
  }, {
    key: "dispose",
    value: function dispose() {
      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function dispose$(_context4) {
        while (1) {
          switch (_context4.prev = _context4.next) {
            case 0:
            case "end":
              return _context4.stop();
          }
        }
      });
    }
  }]);

  return BlocksModule;
}(IDependency);

__decorate([inversify_11(IocManager), __metadata("design:type", IocManager)], BlocksModule.prototype, "iocManager", void 0);

var IRouteProvider =
/*#__PURE__*/
function (_IDependency) {
  _inherits(IRouteProvider, _IDependency);

  function IRouteProvider() {
    _classCallCheck(this, IRouteProvider);

    return _possibleConstructorReturn(this, _getPrototypeOf(IRouteProvider).apply(this, arguments));
  }

  _createClass(IRouteProvider, [{
    key: "getRoutes",
    value: function getRoutes() {
      throw new Error("getRoutes is not implemented.");
    }
  }]);

  return IRouteProvider;
}(IDependency);

var ITemplateProvider =
/*#__PURE__*/
function (_IDependency2) {
  _inherits(ITemplateProvider, _IDependency2);

  function ITemplateProvider() {
    _classCallCheck(this, ITemplateProvider);

    return _possibleConstructorReturn(this, _getPrototypeOf(ITemplateProvider).apply(this, arguments));
  }

  _createClass(ITemplateProvider, [{
    key: "getTemplate",
    value: function getTemplate() {
      throw new Error("getTemplate is not implemented.");
    }
  }]);

  return ITemplateProvider;
}(IDependency);

var RouteResult = function RouteResult() {
  _classCallCheck(this, RouteResult);

  this.layout = "";
  this.path = "";
  this.children = [];
};

var TemplateResult = function TemplateResult() {
  _classCallCheck(this, TemplateResult);
};

var $find = arrayIteration.find;
var FIND = 'find';
var SKIPS_HOLES = true; // Shouldn't skip holes

if (FIND in []) Array(1)[FIND](function () {
  SKIPS_HOLES = false;
}); // `Array.prototype.find` method
// https://tc39.github.io/ecma262/#sec-array.prototype.find

_export({
  target: 'Array',
  proto: true,
  forced: SKIPS_HOLES
}, {
  find: function find(callbackfn
  /* , that = undefined */
  ) {
    return $find(this, callbackfn, arguments.length > 1 ? arguments[1] : undefined);
  }
}); // https://tc39.github.io/ecma262/#sec-array.prototype-@@unscopables

var find = entryVirtual('Array').find;

var ArrayPrototype$3 = Array.prototype;

var find_1 = function (it) {
  var own = it.find;
  return it === ArrayPrototype$3 || it instanceof Array && own === ArrayPrototype$3.find ? find : own;
};

var find$1 = find_1;

var find$2 = find$1;

var nativePromiseConstructor = global_1.Promise;

var ITERATOR$4 = wellKnownSymbol('iterator');
var SAFE_CLOSING = false;

try {
  var called = 0;
  var iteratorWithReturn = {
    next: function () {
      return {
        done: !!called++
      };
    },
    'return': function () {
      SAFE_CLOSING = true;
    }
  };

  iteratorWithReturn[ITERATOR$4] = function () {
    return this;
  }; // eslint-disable-next-line no-throw-literal


  Array.from(iteratorWithReturn, function () {
    throw 2;
  });
} catch (error) {
  /* empty */
}

var checkCorrectnessOfIteration = function (exec, SKIP_CLOSING) {
  if (!SKIP_CLOSING && !SAFE_CLOSING) return false;
  var ITERATION_SUPPORT = false;

  try {
    var object = {};

    object[ITERATOR$4] = function () {
      return {
        next: function () {
          return {
            done: ITERATION_SUPPORT = true
          };
        }
      };
    };

    exec(object);
  } catch (error) {
    /* empty */
  }

  return ITERATION_SUPPORT;
};

var SPECIES$4 = wellKnownSymbol('species'); // `SpeciesConstructor` abstract operation
// https://tc39.github.io/ecma262/#sec-speciesconstructor

var speciesConstructor = function (O, defaultConstructor) {
  var C = anObject(O).constructor;
  var S;
  return C === undefined || (S = anObject(C)[SPECIES$4]) == undefined ? defaultConstructor : aFunction(S);
};

var isIos = /(iphone|ipod|ipad).*applewebkit/i.test(userAgent);

var location = global_1.location;
var set$1 = global_1.setImmediate;
var clear = global_1.clearImmediate;
var process$2 = global_1.process;
var MessageChannel = global_1.MessageChannel;
var Dispatch = global_1.Dispatch;
var counter = 0;
var queue = {};
var ONREADYSTATECHANGE = 'onreadystatechange';
var defer, channel, port;

var run = function (id) {
  // eslint-disable-next-line no-prototype-builtins
  if (queue.hasOwnProperty(id)) {
    var fn = queue[id];
    delete queue[id];
    fn();
  }
};

var runner = function (id) {
  return function () {
    run(id);
  };
};

var listener = function (event) {
  run(event.data);
};

var post = function (id) {
  // old engines have not location.origin
  global_1.postMessage(id + '', location.protocol + '//' + location.host);
}; // Node.js 0.9+ & IE10+ has setImmediate, otherwise:


if (!set$1 || !clear) {
  set$1 = function setImmediate(fn) {
    var args = [];
    var i = 1;

    while (arguments.length > i) args.push(arguments[i++]);

    queue[++counter] = function () {
      // eslint-disable-next-line no-new-func
      (typeof fn == 'function' ? fn : Function(fn)).apply(undefined, args);
    };

    defer(counter);
    return counter;
  };

  clear = function clearImmediate(id) {
    delete queue[id];
  }; // Node.js 0.8-


  if (classofRaw(process$2) == 'process') {
    defer = function (id) {
      process$2.nextTick(runner(id));
    }; // Sphere (JS game engine) Dispatch API

  } else if (Dispatch && Dispatch.now) {
    defer = function (id) {
      Dispatch.now(runner(id));
    }; // Browsers with MessageChannel, includes WebWorkers
    // except iOS - https://github.com/zloirock/core-js/issues/624

  } else if (MessageChannel && !isIos) {
    channel = new MessageChannel();
    port = channel.port2;
    channel.port1.onmessage = listener;
    defer = bindContext(port.postMessage, port, 1); // Browsers with postMessage, skip WebWorkers
    // IE8 has postMessage, but it's sync & typeof its postMessage is 'object'
  } else if (global_1.addEventListener && typeof postMessage == 'function' && !global_1.importScripts && !fails(post)) {
    defer = post;
    global_1.addEventListener('message', listener, false); // IE8-
  } else if (ONREADYSTATECHANGE in documentCreateElement('script')) {
    defer = function (id) {
      html.appendChild(documentCreateElement('script'))[ONREADYSTATECHANGE] = function () {
        html.removeChild(this);
        run(id);
      };
    }; // Rest old browsers

  } else {
    defer = function (id) {
      setTimeout(runner(id), 0);
    };
  }
}

var task = {
  set: set$1,
  clear: clear
};

var getOwnPropertyDescriptor$2 = objectGetOwnPropertyDescriptor.f;
var macrotask = task.set;
var MutationObserver = global_1.MutationObserver || global_1.WebKitMutationObserver;
var process$3 = global_1.process;
var Promise$1 = global_1.Promise;
var IS_NODE = classofRaw(process$3) == 'process'; // Node.js 11 shows ExperimentalWarning on getting `queueMicrotask`

var queueMicrotaskDescriptor = getOwnPropertyDescriptor$2(global_1, 'queueMicrotask');
var queueMicrotask = queueMicrotaskDescriptor && queueMicrotaskDescriptor.value;
var flush, head, last, notify, toggle, node, promise, then; // modern engines have queueMicrotask method

if (!queueMicrotask) {
  flush = function () {
    var parent, fn;
    if (IS_NODE && (parent = process$3.domain)) parent.exit();

    while (head) {
      fn = head.fn;
      head = head.next;

      try {
        fn();
      } catch (error) {
        if (head) notify();else last = undefined;
        throw error;
      }
    }

    last = undefined;
    if (parent) parent.enter();
  }; // Node.js


  if (IS_NODE) {
    notify = function () {
      process$3.nextTick(flush);
    }; // browsers with MutationObserver, except iOS - https://github.com/zloirock/core-js/issues/339

  } else if (MutationObserver && !isIos) {
    toggle = true;
    node = document.createTextNode('');
    new MutationObserver(flush).observe(node, {
      characterData: true
    });

    notify = function () {
      node.data = toggle = !toggle;
    }; // environments with maybe non-completely correct, but existent Promise

  } else if (Promise$1 && Promise$1.resolve) {
    // Promise.resolve without an argument throws an error in LG WebOS 2
    promise = Promise$1.resolve(undefined);
    then = promise.then;

    notify = function () {
      then.call(promise, flush);
    }; // for other environments - macrotask based on:
    // - setImmediate
    // - MessageChannel
    // - window.postMessag
    // - onreadystatechange
    // - setTimeout

  } else {
    notify = function () {
      // strange IE + webpack dev server bug - use .call(global)
      macrotask.call(global_1, flush);
    };
  }
}

var microtask = queueMicrotask || function (fn) {
  var task = {
    fn: fn,
    next: undefined
  };
  if (last) last.next = task;

  if (!head) {
    head = task;
    notify();
  }

  last = task;
};

var PromiseCapability = function (C) {
  var resolve, reject;
  this.promise = new C(function ($$resolve, $$reject) {
    if (resolve !== undefined || reject !== undefined) throw TypeError('Bad Promise constructor');
    resolve = $$resolve;
    reject = $$reject;
  });
  this.resolve = aFunction(resolve);
  this.reject = aFunction(reject);
}; // 25.4.1.5 NewPromiseCapability(C)


var f$7 = function (C) {
  return new PromiseCapability(C);
};

var newPromiseCapability = {
  f: f$7
};

var promiseResolve = function (C, x) {
  anObject(C);
  if (isObject(x) && x.constructor === C) return x;
  var promiseCapability = newPromiseCapability.f(C);
  var resolve = promiseCapability.resolve;
  resolve(x);
  return promiseCapability.promise;
};

var hostReportErrors = function (a, b) {
  var console = global_1.console;

  if (console && console.error) {
    arguments.length === 1 ? console.error(a) : console.error(a, b);
  }
};

var perform = function (exec) {
  try {
    return {
      error: false,
      value: exec()
    };
  } catch (error) {
    return {
      error: true,
      value: error
    };
  }
};

var task$1 = task.set;
var SPECIES$5 = wellKnownSymbol('species');
var PROMISE = 'Promise';
var getInternalState$3 = internalState.get;
var setInternalState$5 = internalState.set;
var getInternalPromiseState = internalState.getterFor(PROMISE);
var PromiseConstructor = nativePromiseConstructor;
var TypeError$1 = global_1.TypeError;
var document$2 = global_1.document;
var process$4 = global_1.process;
var $fetch = getBuiltIn('fetch');
var newPromiseCapability$1 = newPromiseCapability.f;
var newGenericPromiseCapability = newPromiseCapability$1;
var IS_NODE$1 = classofRaw(process$4) == 'process';
var DISPATCH_EVENT = !!(document$2 && document$2.createEvent && global_1.dispatchEvent);
var UNHANDLED_REJECTION = 'unhandledrejection';
var REJECTION_HANDLED = 'rejectionhandled';
var PENDING = 0;
var FULFILLED = 1;
var REJECTED = 2;
var HANDLED = 1;
var UNHANDLED = 2;
var Internal, OwnPromiseCapability, PromiseWrapper;
var FORCED$1 = isForced_1(PROMISE, function () {
  var GLOBAL_CORE_JS_PROMISE = inspectSource(PromiseConstructor) !== String(PromiseConstructor);

  if (!GLOBAL_CORE_JS_PROMISE) {
    // V8 6.6 (Node 10 and Chrome 66) have a bug with resolving custom thenables
    // https://bugs.chromium.org/p/chromium/issues/detail?id=830565
    // We can't detect it synchronously, so just check versions
    if (v8Version === 66) return true; // Unhandled rejections tracking support, NodeJS Promise without it fails @@species test

    if (!IS_NODE$1 && typeof PromiseRejectionEvent != 'function') return true;
  } // We need Promise#finally in the pure version for preventing prototype pollution


  if ( !PromiseConstructor.prototype['finally']) return true; // We can't use @@species feature detection in V8 since it causes
  // deoptimization and performance degradation
  // https://github.com/zloirock/core-js/issues/679

  if (v8Version >= 51 && /native code/.test(PromiseConstructor)) return false; // Detect correctness of subclassing with @@species support

  var promise = PromiseConstructor.resolve(1);

  var FakePromise = function (exec) {
    exec(function () {
      /* empty */
    }, function () {
      /* empty */
    });
  };

  var constructor = promise.constructor = {};
  constructor[SPECIES$5] = FakePromise;
  return !(promise.then(function () {
    /* empty */
  }) instanceof FakePromise);
});
var INCORRECT_ITERATION = FORCED$1 || !checkCorrectnessOfIteration(function (iterable) {
  PromiseConstructor.all(iterable)['catch'](function () {
    /* empty */
  });
}); // helpers

var isThenable = function (it) {
  var then;
  return isObject(it) && typeof (then = it.then) == 'function' ? then : false;
};

var notify$1 = function (promise, state, isReject) {
  if (state.notified) return;
  state.notified = true;
  var chain = state.reactions;
  microtask(function () {
    var value = state.value;
    var ok = state.state == FULFILLED;
    var index = 0; // variable length - can't use forEach

    while (chain.length > index) {
      var reaction = chain[index++];
      var handler = ok ? reaction.ok : reaction.fail;
      var resolve = reaction.resolve;
      var reject = reaction.reject;
      var domain = reaction.domain;
      var result, then, exited;

      try {
        if (handler) {
          if (!ok) {
            if (state.rejection === UNHANDLED) onHandleUnhandled(promise, state);
            state.rejection = HANDLED;
          }

          if (handler === true) result = value;else {
            if (domain) domain.enter();
            result = handler(value); // can throw

            if (domain) {
              domain.exit();
              exited = true;
            }
          }

          if (result === reaction.promise) {
            reject(TypeError$1('Promise-chain cycle'));
          } else if (then = isThenable(result)) {
            then.call(result, resolve, reject);
          } else resolve(result);
        } else reject(value);
      } catch (error) {
        if (domain && !exited) domain.exit();
        reject(error);
      }
    }

    state.reactions = [];
    state.notified = false;
    if (isReject && !state.rejection) onUnhandled(promise, state);
  });
};

var dispatchEvent = function (name, promise, reason) {
  var event, handler;

  if (DISPATCH_EVENT) {
    event = document$2.createEvent('Event');
    event.promise = promise;
    event.reason = reason;
    event.initEvent(name, false, true);
    global_1.dispatchEvent(event);
  } else event = {
    promise: promise,
    reason: reason
  };

  if (handler = global_1['on' + name]) handler(event);else if (name === UNHANDLED_REJECTION) hostReportErrors('Unhandled promise rejection', reason);
};

var onUnhandled = function (promise, state) {
  task$1.call(global_1, function () {
    var value = state.value;
    var IS_UNHANDLED = isUnhandled(state);
    var result;

    if (IS_UNHANDLED) {
      result = perform(function () {
        if (IS_NODE$1) {
          process$4.emit('unhandledRejection', value, promise);
        } else dispatchEvent(UNHANDLED_REJECTION, promise, value);
      }); // Browsers should not trigger `rejectionHandled` event if it was handled here, NodeJS - should

      state.rejection = IS_NODE$1 || isUnhandled(state) ? UNHANDLED : HANDLED;
      if (result.error) throw result.value;
    }
  });
};

var isUnhandled = function (state) {
  return state.rejection !== HANDLED && !state.parent;
};

var onHandleUnhandled = function (promise, state) {
  task$1.call(global_1, function () {
    if (IS_NODE$1) {
      process$4.emit('rejectionHandled', promise);
    } else dispatchEvent(REJECTION_HANDLED, promise, state.value);
  });
};

var bind$3 = function (fn, promise, state, unwrap) {
  return function (value) {
    fn(promise, state, value, unwrap);
  };
};

var internalReject = function (promise, state, value, unwrap) {
  if (state.done) return;
  state.done = true;
  if (unwrap) state = unwrap;
  state.value = value;
  state.state = REJECTED;
  notify$1(promise, state, true);
};

var internalResolve = function (promise, state, value, unwrap) {
  if (state.done) return;
  state.done = true;
  if (unwrap) state = unwrap;

  try {
    if (promise === value) throw TypeError$1("Promise can't be resolved itself");
    var then = isThenable(value);

    if (then) {
      microtask(function () {
        var wrapper = {
          done: false
        };

        try {
          then.call(value, bind$3(internalResolve, promise, wrapper, state), bind$3(internalReject, promise, wrapper, state));
        } catch (error) {
          internalReject(promise, wrapper, error, state);
        }
      });
    } else {
      state.value = value;
      state.state = FULFILLED;
      notify$1(promise, state, false);
    }
  } catch (error) {
    internalReject(promise, {
      done: false
    }, error, state);
  }
}; // constructor polyfill


if (FORCED$1) {
  // 25.4.3.1 Promise(executor)
  PromiseConstructor = function Promise(executor) {
    anInstance(this, PromiseConstructor, PROMISE);
    aFunction(executor);
    Internal.call(this);
    var state = getInternalState$3(this);

    try {
      executor(bind$3(internalResolve, this, state), bind$3(internalReject, this, state));
    } catch (error) {
      internalReject(this, state, error);
    }
  }; // eslint-disable-next-line no-unused-vars


  Internal = function Promise(executor) {
    setInternalState$5(this, {
      type: PROMISE,
      done: false,
      notified: false,
      parent: false,
      reactions: [],
      rejection: false,
      state: PENDING,
      value: undefined
    });
  };

  Internal.prototype = redefineAll(PromiseConstructor.prototype, {
    // `Promise.prototype.then` method
    // https://tc39.github.io/ecma262/#sec-promise.prototype.then
    then: function then(onFulfilled, onRejected) {
      var state = getInternalPromiseState(this);
      var reaction = newPromiseCapability$1(speciesConstructor(this, PromiseConstructor));
      reaction.ok = typeof onFulfilled == 'function' ? onFulfilled : true;
      reaction.fail = typeof onRejected == 'function' && onRejected;
      reaction.domain = IS_NODE$1 ? process$4.domain : undefined;
      state.parent = true;
      state.reactions.push(reaction);
      if (state.state != PENDING) notify$1(this, state, false);
      return reaction.promise;
    },
    // `Promise.prototype.catch` method
    // https://tc39.github.io/ecma262/#sec-promise.prototype.catch
    'catch': function (onRejected) {
      return this.then(undefined, onRejected);
    }
  });

  OwnPromiseCapability = function () {
    var promise = new Internal();
    var state = getInternalState$3(promise);
    this.promise = promise;
    this.resolve = bind$3(internalResolve, promise, state);
    this.reject = bind$3(internalReject, promise, state);
  };

  newPromiseCapability.f = newPromiseCapability$1 = function (C) {
    return C === PromiseConstructor || C === PromiseWrapper ? new OwnPromiseCapability(C) : newGenericPromiseCapability(C);
  };
}

_export({
  global: true,
  wrap: true,
  forced: FORCED$1
}, {
  Promise: PromiseConstructor
});
setToStringTag(PromiseConstructor, PROMISE, false, true);
setSpecies(PROMISE);
PromiseWrapper = getBuiltIn(PROMISE); // statics

_export({
  target: PROMISE,
  stat: true,
  forced: FORCED$1
}, {
  // `Promise.reject` method
  // https://tc39.github.io/ecma262/#sec-promise.reject
  reject: function reject(r) {
    var capability = newPromiseCapability$1(this);
    capability.reject.call(undefined, r);
    return capability.promise;
  }
});
_export({
  target: PROMISE,
  stat: true,
  forced: isPure 
}, {
  // `Promise.resolve` method
  // https://tc39.github.io/ecma262/#sec-promise.resolve
  resolve: function resolve(x) {
    return promiseResolve( this === PromiseWrapper ? PromiseConstructor : this, x);
  }
});
_export({
  target: PROMISE,
  stat: true,
  forced: INCORRECT_ITERATION
}, {
  // `Promise.all` method
  // https://tc39.github.io/ecma262/#sec-promise.all
  all: function all(iterable) {
    var C = this;
    var capability = newPromiseCapability$1(C);
    var resolve = capability.resolve;
    var reject = capability.reject;
    var result = perform(function () {
      var $promiseResolve = aFunction(C.resolve);
      var values = [];
      var counter = 0;
      var remaining = 1;
      iterate_1(iterable, function (promise) {
        var index = counter++;
        var alreadyCalled = false;
        values.push(undefined);
        remaining++;
        $promiseResolve.call(C, promise).then(function (value) {
          if (alreadyCalled) return;
          alreadyCalled = true;
          values[index] = value;
          --remaining || resolve(values);
        }, reject);
      });
      --remaining || resolve(values);
    });
    if (result.error) reject(result.value);
    return capability.promise;
  },
  // `Promise.race` method
  // https://tc39.github.io/ecma262/#sec-promise.race
  race: function race(iterable) {
    var C = this;
    var capability = newPromiseCapability$1(C);
    var reject = capability.reject;
    var result = perform(function () {
      var $promiseResolve = aFunction(C.resolve);
      iterate_1(iterable, function (promise) {
        $promiseResolve.call(C, promise).then(capability.resolve, reject);
      });
    });
    if (result.error) reject(result.value);
    return capability.promise;
  }
});

// https://github.com/tc39/proposal-promise-allSettled


_export({
  target: 'Promise',
  stat: true
}, {
  allSettled: function allSettled(iterable) {
    var C = this;
    var capability = newPromiseCapability.f(C);
    var resolve = capability.resolve;
    var reject = capability.reject;
    var result = perform(function () {
      var promiseResolve = aFunction(C.resolve);
      var values = [];
      var counter = 0;
      var remaining = 1;
      iterate_1(iterable, function (promise) {
        var index = counter++;
        var alreadyCalled = false;
        values.push(undefined);
        remaining++;
        promiseResolve.call(C, promise).then(function (value) {
          if (alreadyCalled) return;
          alreadyCalled = true;
          values[index] = {
            status: 'fulfilled',
            value: value
          };
          --remaining || resolve(values);
        }, function (e) {
          if (alreadyCalled) return;
          alreadyCalled = true;
          values[index] = {
            status: 'rejected',
            reason: e
          };
          --remaining || resolve(values);
        });
      });
      --remaining || resolve(values);
    });
    if (result.error) reject(result.value);
    return capability.promise;
  }
});

var NON_GENERIC = !!nativePromiseConstructor && fails(function () {
  nativePromiseConstructor.prototype['finally'].call({
    then: function () {
      /* empty */
    }
  }, function () {
    /* empty */
  });
}); // `Promise.prototype.finally` method
// https://tc39.github.io/ecma262/#sec-promise.prototype.finally

_export({
  target: 'Promise',
  proto: true,
  real: true,
  forced: NON_GENERIC
}, {
  'finally': function (onFinally) {
    var C = speciesConstructor(this, getBuiltIn('Promise'));
    var isFunction = typeof onFinally == 'function';
    return this.then(isFunction ? function (x) {
      return promiseResolve(C, onFinally()).then(function () {
        return x;
      });
    } : onFinally, isFunction ? function (e) {
      return promiseResolve(C, onFinally()).then(function () {
        throw e;
      });
    } : onFinally);
  }
}); // patch native Promise.prototype for native async functions

var promise$1 = path.Promise;

var promise$2 = promise$1;

var promise$3 = promise$2;

var nativeAssign = Object.assign;
var defineProperty$4 = Object.defineProperty; // `Object.assign` method
// https://tc39.github.io/ecma262/#sec-object.assign

var objectAssign = !nativeAssign || fails(function () {
  // should have correct order of operations (Edge bug)
  if (descriptors && nativeAssign({
    b: 1
  }, nativeAssign(defineProperty$4({}, 'a', {
    enumerable: true,
    get: function () {
      defineProperty$4(this, 'b', {
        value: 3,
        enumerable: false
      });
    }
  }), {
    b: 2
  })).b !== 1) return true; // should work with symbols and should have deterministic property order (V8 bug)

  var A = {};
  var B = {}; // eslint-disable-next-line no-undef

  var symbol = Symbol();
  var alphabet = 'abcdefghijklmnopqrst';
  A[symbol] = 7;
  alphabet.split('').forEach(function (chr) {
    B[chr] = chr;
  });
  return nativeAssign({}, A)[symbol] != 7 || objectKeys(nativeAssign({}, B)).join('') != alphabet;
}) ? function assign(target, source) {
  // eslint-disable-line no-unused-vars
  var T = toObject(target);
  var argumentsLength = arguments.length;
  var index = 1;
  var getOwnPropertySymbols = objectGetOwnPropertySymbols.f;
  var propertyIsEnumerable = objectPropertyIsEnumerable.f;

  while (argumentsLength > index) {
    var S = indexedObject(arguments[index++]);
    var keys = getOwnPropertySymbols ? objectKeys(S).concat(getOwnPropertySymbols(S)) : objectKeys(S);
    var length = keys.length;
    var j = 0;
    var key;

    while (length > j) {
      key = keys[j++];
      if (!descriptors || propertyIsEnumerable.call(S, key)) T[key] = S[key];
    }
  }

  return T;
} : nativeAssign;

// https://tc39.github.io/ecma262/#sec-object.assign

_export({
  target: 'Object',
  stat: true,
  forced: Object.assign !== objectAssign
}, {
  assign: objectAssign
});

var assign = path.Object.assign;

var assign$1 = assign;

var assign$2 = assign$1;

var slice$4 = [].slice;
var MSIE = /MSIE .\./.test(userAgent); // <- dirty ie9- check

var wrap$1 = function (scheduler) {
  return function (handler, timeout
  /* , ...arguments */
  ) {
    var boundArgs = arguments.length > 2;
    var args = boundArgs ? slice$4.call(arguments, 2) : undefined;
    return scheduler(boundArgs ? function () {
      // eslint-disable-next-line no-new-func
      (typeof handler == 'function' ? handler : Function(handler)).apply(this, args);
    } : handler, timeout);
  };
}; // ie9- setTimeout & setInterval additional parameters fix
// https://html.spec.whatwg.org/multipage/timers-and-user-prompts.html#timers


_export({
  global: true,
  bind: true,
  forced: MSIE
}, {
  // `setTimeout` method
  // https://html.spec.whatwg.org/multipage/timers-and-user-prompts.html#dom-settimeout
  setTimeout: wrap$1(global_1.setTimeout),
  // `setInterval` method
  // https://html.spec.whatwg.org/multipage/timers-and-user-prompts.html#dom-setinterval
  setInterval: wrap$1(global_1.setInterval)
});

var setTimeout$1 = path.setTimeout;

var setTimeout$2 = setTimeout$1;

var isPromise = function isPromise(obj) {
  return !!obj && (_typeof(obj) === 'object' || typeof obj === 'function') && typeof obj.then === 'function';
};

var MATCH = wellKnownSymbol('match'); // `IsRegExp` abstract operation
// https://tc39.github.io/ecma262/#sec-isregexp

var isRegexp = function (it) {
  var isRegExp;
  return isObject(it) && ((isRegExp = it[MATCH]) !== undefined ? !!isRegExp : classofRaw(it) == 'RegExp');
};

var notARegexp = function (it) {
  if (isRegexp(it)) {
    throw TypeError("The method doesn't accept regular expressions");
  }

  return it;
};

var MATCH$1 = wellKnownSymbol('match');

var correctIsRegexpLogic = function (METHOD_NAME) {
  var regexp = /./;

  try {
    '/./'[METHOD_NAME](regexp);
  } catch (e) {
    try {
      regexp[MATCH$1] = false;
      return '/./'[METHOD_NAME](regexp);
    } catch (f) {
      /* empty */
    }
  }

  return false;
};

var nativeStartsWith = ''.startsWith;
var min$2 = Math.min;
var CORRECT_IS_REGEXP_LOGIC = correctIsRegexpLogic('startsWith'); // https://github.com/zloirock/core-js/pull/702
// https://tc39.github.io/ecma262/#sec-string.prototype.startswith

_export({
  target: 'String',
  proto: true,
  forced:  !CORRECT_IS_REGEXP_LOGIC
}, {
  startsWith: function startsWith(searchString
  /* , position = 0 */
  ) {
    var that = String(requireObjectCoercible(this));
    notARegexp(searchString);
    var index = toLength(min$2(arguments.length > 1 ? arguments[1] : undefined, that.length));
    var search = String(searchString);
    return nativeStartsWith ? nativeStartsWith.call(that, search, index) : that.slice(index, index + search.length) === search;
  }
});

var startsWith = entryVirtual('String').startsWith;

var StringPrototype = String.prototype;

var startsWith_1 = function (it) {
  var own = it.startsWith;
  return typeof it === 'string' || it === StringPrototype || it instanceof String && own === StringPrototype.startsWith ? startsWith : own;
};

var startsWith$1 = startsWith_1;

var startsWith$2 = startsWith$1;

// https://tc39.github.io/ecma262/#sec-array.isarray

_export({
  target: 'Array',
  stat: true
}, {
  isArray: isArray
});

var isArray$1 = path.Array.isArray;

var isArray$2 = isArray$1;

var isArray$3 = isArray$2;

var RouteManager =
/*#__PURE__*/
function () {
  function RouteManager(IocManager) {
    _classCallCheck(this, RouteManager);

    this.iocManager = IocManager;
  }

  _createClass(RouteManager, [{
    key: "getUniquePath",
    value: function getUniquePath() {
      var routes = this.getRoute();
      var routePaths = [];
      var _iteratorNormalCompletion = true;
      var _didIteratorError = false;
      var _iteratorError = undefined;

      try {
        for (var _iterator = getIterator$1(routes), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
          var route = _step.value;

          if (!route.children) {
            routePaths.push(assign$2(true, {}, route, {
              meta: {
                uniqueKey: route.uniqueKey
              }
            }));
            continue;
          }

          var _iteratorNormalCompletion2 = true;
          var _didIteratorError2 = false;
          var _iteratorError2 = undefined;

          try {
            for (var _iterator2 = getIterator$1(route.children), _step2; !(_iteratorNormalCompletion2 = (_step2 = _iterator2.next()).done); _iteratorNormalCompletion2 = true) {
              var routeChild = _step2.value;

              if (!routeChild.children) {
                routePaths.push(assign$2(true, routeChild, {
                  path: route.path + "/" + routeChild.path,
                  meta: {
                    uniqueKey: routeChild.uniqueKey
                  }
                }));
                continue;
              }

              var _iteratorNormalCompletion3 = true;
              var _didIteratorError3 = false;
              var _iteratorError3 = undefined;

              try {
                for (var _iterator3 = getIterator$1(routeChild.children), _step3; !(_iteratorNormalCompletion3 = (_step3 = _iterator3.next()).done); _iteratorNormalCompletion3 = true) {
                  var routeChildTemp = _step3.value;

                  if (!routeChildTemp.children) {
                    routePaths.push(assign$2(true, routeChild, {
                      path: route.path + "/" + routeChild.path + "/" + routeChildTemp.path,
                      meta: {
                        uniqueKey: routeChildTemp.uniqueKey
                      }
                    }));
                    continue;
                  }
                }
              } catch (err) {
                _didIteratorError3 = true;
                _iteratorError3 = err;
              } finally {
                try {
                  if (!_iteratorNormalCompletion3 && _iterator3.return != null) {
                    _iterator3.return();
                  }
                } finally {
                  if (_didIteratorError3) {
                    throw _iteratorError3;
                  }
                }
              }
            }
          } catch (err) {
            _didIteratorError2 = true;
            _iteratorError2 = err;
          } finally {
            try {
              if (!_iteratorNormalCompletion2 && _iterator2.return != null) {
                _iterator2.return();
              }
            } finally {
              if (_didIteratorError2) {
                throw _iteratorError2;
              }
            }
          }
        }
      } catch (err) {
        _didIteratorError = true;
        _iteratorError = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion && _iterator.return != null) {
            _iterator.return();
          }
        } finally {
          if (_didIteratorError) {
            throw _iteratorError;
          }
        }
      }

      return routePaths;
    }
  }, {
    key: "getRoute",
    value: function getRoute() {
      var routeObj = [];
      var templateProviders = this.iocManager.isRegistered(Types.ITemplateProvider) ? this.iocManager.getAll(Types.ITemplateProvider) : [];
      if (!this.iocManager.isRegistered(Types.IRouteProvider)) return routeObj;
      var _iteratorNormalCompletion4 = true;
      var _didIteratorError4 = false;
      var _iteratorError4 = undefined;

      try {
        for (var _iterator4 = getIterator$1(this.iocManager.getAll(Types.IRouteProvider)), _step4; !(_iteratorNormalCompletion4 = (_step4 = _iterator4.next()).done); _iteratorNormalCompletion4 = true) {
          var routeProvider = _step4.value;
          var moduleName = this.iocManager.get(Types.IBlocksShell).typeMapModuleName.get(routeProvider.constructor);
          var routes = routeProvider.getRoutes();
          if (!routes || !isArray$3(routes)) continue;
          var layoutRouteChilds = {};
          var _iteratorNormalCompletion5 = true;
          var _didIteratorError5 = false;
          var _iteratorError5 = undefined;

          try {
            var _loop = function _loop() {
              var _context2;

              var route = _step5.value;
              route.path = moduleName + "/" + route.path;

              if (route.uniqueKey) {
                route.uniqueKey = RouteManager.getUniqueKey(moduleName, route.uniqueKey); //  route.uniqueKey + "_" + moduleName;

                route.meta = assign$2(true, route.meta, {
                  uniqueKey: route.uniqueKey
                });
              }

              if (route.layout) {
                layoutRouteChilds[route.layout] = layoutRouteChilds[route.layout] ? layoutRouteChilds[route.layout] : new RouteResult();
                var _tempRoute = layoutRouteChilds[route.layout];
                var _iteratorNormalCompletion6 = true;
                var _didIteratorError6 = false;
                var _iteratorError6 = undefined;

                try {
                  for (var _iterator6 = getIterator$1(templateProviders), _step6; !(_iteratorNormalCompletion6 = (_step6 = _iterator6.next()).done); _iteratorNormalCompletion6 = true) {
                    var _context;

                    var templateProvider = _step6.value;

                    var template = find$2(_context = templateProvider.getTemplate()).call(_context, function (t) {
                      return t.name === route.layout;
                    });

                    if (template) {
                      _tempRoute = assign$2(_tempRoute, template);
                      route.name = route.name ? route.name : route.uniqueKey;

                      _tempRoute.children.push(route);

                      break;
                    }
                  }
                } catch (err) {
                  _didIteratorError6 = true;
                  _iteratorError6 = err;
                } finally {
                  try {
                    if (!_iteratorNormalCompletion6 && _iterator6.return != null) {
                      _iterator6.return();
                    }
                  } finally {
                    if (_didIteratorError6) {
                      throw _iteratorError6;
                    }
                  }
                }

                if (_tempRoute.children.length < 1) routeObj.push(route);
                return "continue";
              }

              if (!startsWith$2(_context2 = route.path).call(_context2, "/")) route.path = "/" + route.path;
              routeObj.push(route);
            };

            for (var _iterator5 = getIterator$1(routes), _step5; !(_iteratorNormalCompletion5 = (_step5 = _iterator5.next()).done); _iteratorNormalCompletion5 = true) {
              var _ret = _loop();

              if (_ret === "continue") continue;
            }
          } catch (err) {
            _didIteratorError5 = true;
            _iteratorError5 = err;
          } finally {
            try {
              if (!_iteratorNormalCompletion5 && _iterator5.return != null) {
                _iterator5.return();
              }
            } finally {
              if (_didIteratorError5) {
                throw _iteratorError5;
              }
            }
          }

          for (var layoutRouteKey in layoutRouteChilds) {
            if (layoutRouteChilds.hasOwnProperty(layoutRouteKey)) {
              var tempRoute = layoutRouteChilds[layoutRouteKey];
              routeObj.push(assign$2({}, tempRoute));
            }
          }
        }
      } catch (err) {
        _didIteratorError4 = true;
        _iteratorError4 = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion4 && _iterator4.return != null) {
            _iterator4.return();
          }
        } finally {
          if (_didIteratorError4) {
            throw _iteratorError4;
          }
        }
      }

      return routeObj;
    }
  }]);

  return RouteManager;
}();

RouteManager.getUniqueKey = function (moduleName, uniqueKey) {
  return uniqueKey + "_" + moduleName;
};

RouteManager = __decorate([__param(0, inversify_11(IocManager)), __metadata("design:paramtypes", [IocManager])], RouteManager);
var DefaultRouteManager = RouteManager;

var Controller =
/*#__PURE__*/
function (_Vue) {
  _inherits(Controller, _Vue);

  function Controller() {
    var _this2;

    _classCallCheck(this, Controller);

    _this2 = _possibleConstructorReturn(this, _getPrototypeOf(Controller).call(this));
    _this2.UniqueKey = "";
    _this2.switchMode = 0;
    return _this2;
  }

  _createClass(Controller, [{
    key: "created",
    value: function created() {
      this.$emit("beforeViewWillEnter", this);
      this.viewWillEnterResult = this.viewWillEnter();
      console.log("assign viewWillEnterResult");

      if ((typeof this.UniqueKey === "undefined" || this.UniqueKey === "") && this.$route && this.$route.meta && this.$route.meta.uniqueKey) {
        this.UniqueKey = this.$route.meta.uniqueKey;
      }
    }
  }, {
    key: "mounted",
    value: function mounted() {
      var _this3 = this;

      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function mounted$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              this.$emit("ViewDidEnterStart", this);

              if (!isPromise(this.viewDidEnter)) {
                _context.next = 7;
                break;
              }

              this.viewDidEnterResult = this.viewDidEnter();
              _context.next = 5;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(this.viewDidEnterResult);

            case 5:
              _context.next = 8;
              break;

            case 7:
              this.viewDidEnter();

            case 8:
              this.viewAnimationEndTime = setTimeout$2(function () {
                _this3.viewAnimationEnd();
              }, 800);

            case 9:
            case "end":
              return _context.stop();
          }
        }
      }, null, this);
    }
  }, {
    key: "beforeMount",
    value: function beforeMount() {
      this.$emit("beforeViewDidEnter", this);
      this.beforeViewDidEnter();
    }
  }, {
    key: "beforeViewDidEnter",
    value: function beforeViewDidEnter() {}
  }, {
    key: "viewAnimationEndAndDataReady",
    value: function viewAnimationEndAndDataReady() {}
  }, {
    key: "viewAnimationEnd",
    value: function viewAnimationEnd() {
      var _this;

      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function viewAnimationEnd$(_context3) {
        while (1) {
          switch (_context3.prev = _context3.next) {
            case 0:
              console.debug("viewAnimationEnd start");
              clearTimeout(this.viewAnimationEndTime);

              if (!isPromise(this.viewWillEnterResult)) {
                _context3.next = 6;
                break;
              }

              _context3.next = 5;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(this.viewWillEnterResult);

            case 5:
              console.debug("await  viewWillEnterResult");

            case 6:
              console.debug("viewWillEnterResult end");

              if (!isPromise(this.viewAnimationEndAndDataReady)) {
                _context3.next = 12;
                break;
              }

              _context3.next = 10;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(this.viewAnimationEndAndDataReady());

            case 10:
              _context3.next = 13;
              break;

            case 12:
              this.viewAnimationEndAndDataReady();

            case 13:
              if (!isPromise(this.viewDidEnterResult)) {
                _context3.next = 17;
                break;
              }

              _context3.next = 16;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(this.viewDidEnterResult);

            case 16:
              console.debug("await  viewDidEnterResult");

            case 17:
              console.debug("viewAnimationEndAndDataReady end"); // this.$emit("viewDataReadyFinish")

              _this = this;

              _this.$emit("viewDataReadyFinish");

              _this.$nextTick(function _callee() {
                var allCom, _iteratorNormalCompletion, _didIteratorError, _iteratorError, _iterator, _step, com, comChild;

                return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function _callee$(_context2) {
                  while (1) {
                    switch (_context2.prev = _context2.next) {
                      case 0:
                        if (!_this.$el.querySelectorAll) {
                          _context2.next = 30;
                          break;
                        }

                        allCom = _this.$el.querySelectorAll(".vue-recycle-scroller__item-view");
                        _iteratorNormalCompletion = true;
                        _didIteratorError = false;
                        _iteratorError = undefined;
                        _context2.prev = 5;
                        _iterator = getIterator$1(allCom);

                      case 7:
                        if (_iteratorNormalCompletion = (_step = _iterator.next()).done) {
                          _context2.next = 16;
                          break;
                        }

                        com = _step.value;
                        comChild = com.firstElementChild;

                        if (!(comChild && comChild.componentOnReady)) {
                          _context2.next = 13;
                          break;
                        }

                        _context2.next = 13;
                        return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(comChild.componentOnReady());

                      case 13:
                        _iteratorNormalCompletion = true;
                        _context2.next = 7;
                        break;

                      case 16:
                        _context2.next = 22;
                        break;

                      case 18:
                        _context2.prev = 18;
                        _context2.t0 = _context2["catch"](5);
                        _didIteratorError = true;
                        _iteratorError = _context2.t0;

                      case 22:
                        _context2.prev = 22;
                        _context2.prev = 23;

                        if (!_iteratorNormalCompletion && _iterator.return != null) {
                          _iterator.return();
                        }

                      case 25:
                        _context2.prev = 25;

                        if (!_didIteratorError) {
                          _context2.next = 28;
                          break;
                        }

                        throw _iteratorError;

                      case 28:
                        return _context2.finish(25);

                      case 29:
                        return _context2.finish(22);

                      case 30:
                        _this.$emit("viewReaderFinish", _this);

                      case 31:
                      case "end":
                        return _context2.stop();
                    }
                  }
                }, null, null, [[5, 18, 22, 30], [23,, 25, 29]]);
              });

            case 21:
            case "end":
              return _context3.stop();
          }
        }
      }, null, this);
    }
  }, {
    key: "viewWillEnter",
    value: function viewWillEnter() {}
  }, {
    key: "viewDidEnter",
    value: function viewDidEnter() {}
  }, {
    key: "switch",
    value: function _switch(switchUniqueKey, switchParams) {
      var _this4 = this;

      var routeManager, uniqueKey, route, _iteratorNormalCompletion2, _didIteratorError2, _iteratorError2, _iterator2, _step2, _context4, layoutRoutes, dialogParams, dialog, routeReslove, routeReject, routePushPromise;

      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function _switch$(_context5) {
        while (1) {
          switch (_context5.prev = _context5.next) {
            case 0:
              if (!(_typeof(switchParams) === undefined || switchParams === null || _typeof(switchUniqueKey) === undefined || switchUniqueKey.uniqueKey === null)) {
                _context5.next = 2;
                break;
              }

              throw new Error("switchParams must contains uniqueKey.");

            case 2:
              routeManager = globalIocManager.get(Types.IRouteManager);
              uniqueKey = switchUniqueKey.moduleName ? DefaultRouteManager.getUniqueKey(switchUniqueKey.moduleName, switchUniqueKey.uniqueKey) : DefaultRouteManager.getUniqueKey(this.getModuleName(), switchUniqueKey.uniqueKey);
              _iteratorNormalCompletion2 = true;
              _didIteratorError2 = false;
              _iteratorError2 = undefined;
              _context5.prev = 7;
              _iterator2 = getIterator$1(routeManager.getRoute());

            case 9:
              if (_iteratorNormalCompletion2 = (_step2 = _iterator2.next()).done) {
                _context5.next = 17;
                break;
              }

              layoutRoutes = _step2.value;
              route = find$2(_context4 = layoutRoutes.children).call(_context4, function (r) {
                return r.uniqueKey === uniqueKey;
              });

              if (!route) {
                _context5.next = 14;
                break;
              }

              return _context5.abrupt("break", 17);

            case 14:
              _iteratorNormalCompletion2 = true;
              _context5.next = 9;
              break;

            case 17:
              _context5.next = 23;
              break;

            case 19:
              _context5.prev = 19;
              _context5.t0 = _context5["catch"](7);
              _didIteratorError2 = true;
              _iteratorError2 = _context5.t0;

            case 23:
              _context5.prev = 23;
              _context5.prev = 24;

              if (!_iteratorNormalCompletion2 && _iterator2.return != null) {
                _iterator2.return();
              }

            case 26:
              _context5.prev = 26;

              if (!_didIteratorError2) {
                _context5.next = 29;
                break;
              }

              throw _iteratorError2;

            case 29:
              return _context5.finish(26);

            case 30:
              return _context5.finish(23);

            case 31:
              if (!(this.switchMode === 0)) {
                _context5.next = 42;
                break;
              }

              dialogParams = assign$2(true, {
                component: route.component
              }, switchParams);
              dialogParams.componentProps = assign$2({
                UniqueKey: uniqueKey
              }, dialogParams.componentProps);
              _context5.next = 36;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(this.$dialog.create(dialogParams));

            case 36:
              dialog = _context5.sent;
              _context5.next = 39;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(dialog.present());

            case 39:
              return _context5.abrupt("return", _context5.sent);

            case 42:
              routePushPromise = new promise$3(function (resolve, reject) {
                routeReslove = resolve;
                routeReject = reject;
              });
              this.$router.push({
                name: route.name
              }, function () {
                routeReslove({
                  onDidDismiss: new promise$3(function (resolve, reject) {
                    _this4.switchResolve = resolve;
                  })
                });
              }, function () {
                routeReject();
              });
              _context5.next = 46;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(routePushPromise);

            case 46:
              return _context5.abrupt("return", _context5.sent);

            case 47:
            case "end":
              return _context5.stop();
          }
        }
      }, null, this, [[7, 19, 23, 31], [24,, 26, 30]]);
    }
  }, {
    key: "exit",
    value: function exit() {
      if (this.switchMode === 0) {
        var _context6;

        for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
          args[_key] = arguments[_key];
        }

        this.$emit.apply(this, concat$2(_context6 = ["exit"]).call(_context6, args));
      } else {
        if (this.switchResolve) {
          this.switchResolve();
        }
      }
    }
  }, {
    key: "getModuleName",
    value: function getModuleName() {
      var shell = globalIocManager.get(Types.IBlocksShell);
      var moduleName = shell.typeMapModuleName.get(this.$options.type);
      if (moduleName === undefined) throw new Error(this + " not belong to framework.");
      return moduleName;
    }
  }, {
    key: "getResources",
    value: function getResources(moduleName, resourceType, resourceName) {
      var _context7;

      var resourceManager = globalIocManager.get(Types.IResourceManager);
      var resourceDef = resourceManager.getResource(moduleName, resourceType, resourceName);
      if (resourceDef === undefined) throw new Error(concat$2(_context7 = "Can not found resourceType:".concat(resourceType, ", resourceName:")).call(_context7, resourceName, " in resources."));
      return resourceDef.resource;
    }
  }]);

  return Controller;
}(Vue);

__decorate([Prop({
  type: String
}), __metadata("design:type", String)], Controller.prototype, "UniqueKey", void 0);

Controller = __decorate([Component$1({}), __metadata("design:paramtypes", [])], Controller);

var Component = function Component(options) {
  return ComponentFactory(Component$1, options);
};

function ComponentFactory(oldComponentFactory, options) {
  var com = oldComponentFactory(options);

  if (typeof options === 'function') {
    com.options.type = options;
    return com;
  }

  return function (Component) {
    var comObj = com(Component);
    if (typeof Component === 'function') comObj.options.type = options;
    return comObj;
  };
}

var IManifestProvider =
/*#__PURE__*/
function (_IDependency) {
  _inherits(IManifestProvider, _IDependency);

  function IManifestProvider() {
    _classCallCheck(this, IManifestProvider);

    return _possibleConstructorReturn(this, _getPrototypeOf(IManifestProvider).apply(this, arguments));
  }

  _createClass(IManifestProvider, [{
    key: "buildManifests",
    value: function buildManifests(builder) {
      throw new Error("buildManifests is not implemented.");
    }
  }]);

  return IManifestProvider;
}(IDependency);

var ResourceManifest =
/*#__PURE__*/
function () {
  function ResourceManifest() {
    _classCallCheck(this, ResourceManifest);

    this.resources = [];
  }

  _createClass(ResourceManifest, [{
    key: "defineResource",
    value: function defineResource(resourceType, resourceName) {
      var manifest = new ResourceDefinition(resourceType, resourceName);
      this.resources.push(manifest);
      return manifest;
    }
  }]);

  return ResourceManifest;
}();

var ResourceDefinition =
/*#__PURE__*/
function () {
  function ResourceDefinition(resourceType, resourceName) {
    _classCallCheck(this, ResourceDefinition);

    this.resourceName = resourceName;
    this.resourceType = resourceType;
  }

  _createClass(ResourceDefinition, [{
    key: "add",
    value: function add(resource) {
      this.resource = resource;
    }
  }]);

  return ResourceDefinition;
}();

var ResourceManifestBuilder =
/*#__PURE__*/
function () {
  function ResourceManifestBuilder() {
    _classCallCheck(this, ResourceManifestBuilder);

    this.resourceManifests = [];
  }

  _createClass(ResourceManifestBuilder, [{
    key: "add",
    value: function add() {
      var manifest = new ResourceManifest();
      this.resourceManifests.push(manifest);
      return manifest;
    }
  }]);

  return ResourceManifestBuilder;
}();

var decorateIfNoExist = function decorateIfNoExist(decorator, target, parameterIndex) {
  if (Reflect.hasOwnMetadata(inversify_1.PARAM_TYPES, target)) return;
  return inversify_20(decorator, target, parameterIndex);
};

//  let IDependency = IDependencyDefine;
// this.Interface = {
//     startupModule, IDependency, IShell, IRouteProvider, ITemplateProvider, Types,
//     inject, BlocksModule, Controller, Component, RouteResult, TemplateResult, globalIocManager,IBootstrapper
// }  

function asyncCompatible() {
  return function (target, propertyKey, descriptor) {
    var originFunc = descriptor.value;

    descriptor.value = function _callee() {
      var _context;

      var _len,
          param,
          _key,
          paramLength,
          actParam,
          p,
          returnObj,
          hasThen,
          result,
          _args = arguments;

      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function _callee$(_context2) {
        while (1) {
          switch (_context2.prev = _context2.next) {
            case 0:
              for (_len = _args.length, param = new Array(_len), _key = 0; _key < _len; _key++) {
                param[_key] = _args[_key];
              }

              paramLength = param.length; //debugger;

              actParam = paramLength > 1 ? slice$3(param).call(param, 0, paramLength) : param;
              p = paramLength > 0 ? param[paramLength - 1] : null;
              returnObj = originFunc.call.apply(originFunc, concat$2(_context = [this]).call(_context, _toConsumableArray(actParam)));
              hasThen = true;

              try {
                hasThen = returnObj.then !== "undefined";
              } catch (error) {
                hasThen = false;
              }

              if (!hasThen) {
                _context2.next = 17;
                break;
              }

              _context2.prev = 8;
              _context2.next = 11;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(returnObj);

            case 11:
              result = _context2.sent;

            case 12:
              _context2.prev = 12;
              p && p(result);
              return _context2.finish(12);

            case 15:
              _context2.next = 18;
              break;

            case 17:
              p && p(returnObj);

            case 18:
            case "end":
              return _context2.stop();
          }
        }
      }, null, this, [[8,, 12, 15]]);
    };
  };
}

var values = entryVirtual('Array').values;

var values$1 = values;

var ArrayPrototype$4 = Array.prototype;
var DOMIterables = {
  DOMTokenList: true,
  NodeList: true
};

var values_1 = function (it) {
  var own = it.values;
  return it === ArrayPrototype$4 || it instanceof Array && own === ArrayPrototype$4.values // eslint-disable-next-line no-prototype-builtins
  || DOMIterables.hasOwnProperty(classof(it)) ? values$1 : own;
};

var values$2 = values_1;

var keys$1 = entryVirtual('Array').keys;

var keys$2 = keys$1;

var ArrayPrototype$5 = Array.prototype;
var DOMIterables$1 = {
  DOMTokenList: true,
  NodeList: true
};

var keys_1 = function (it) {
  var own = it.keys;
  return it === ArrayPrototype$5 || it instanceof Array && own === ArrayPrototype$5.keys // eslint-disable-next-line no-prototype-builtins
  || DOMIterables$1.hasOwnProperty(classof(it)) ? keys$2 : own;
};

var keys$3 = keys_1;

var FAILS_ON_PRIMITIVES = fails(function () {
  objectKeys(1);
}); // `Object.keys` method
// https://tc39.github.io/ecma262/#sec-object.keys

_export({
  target: 'Object',
  stat: true,
  forced: FAILS_ON_PRIMITIVES
}, {
  keys: function keys(it) {
    return objectKeys(toObject(it));
  }
});

var keys$4 = path.Object.keys;

var keys$5 = keys$4;

var keys$6 = keys$5;

var $filter = arrayIteration.filter;
var HAS_SPECIES_SUPPORT = arrayMethodHasSpeciesSupport('filter'); // Edge 14- issue

var USES_TO_LENGTH = HAS_SPECIES_SUPPORT && !fails(function () {
  [].filter.call({
    length: -1,
    0: 1
  }, function (it) {
    throw it;
  });
}); // `Array.prototype.filter` method
// https://tc39.github.io/ecma262/#sec-array.prototype.filter
// with adding support of @@species

_export({
  target: 'Array',
  proto: true,
  forced: !HAS_SPECIES_SUPPORT || !USES_TO_LENGTH
}, {
  filter: function filter(callbackfn
  /* , thisArg */
  ) {
    return $filter(this, callbackfn, arguments.length > 1 ? arguments[1] : undefined);
  }
});

var filter = entryVirtual('Array').filter;

var ArrayPrototype$6 = Array.prototype;

var filter_1 = function (it) {
  var own = it.filter;
  return it === ArrayPrototype$6 || it instanceof Array && own === ArrayPrototype$6.filter ? filter : own;
};

var filter$1 = filter_1;

var filter$2 = filter$1;

var $includes = arrayIncludes.includes; // `Array.prototype.includes` method
// https://tc39.github.io/ecma262/#sec-array.prototype.includes

_export({
  target: 'Array',
  proto: true
}, {
  includes: function includes(el
  /* , fromIndex = 0 */
  ) {
    return $includes(this, el, arguments.length > 1 ? arguments[1] : undefined);
  }
}); // https://tc39.github.io/ecma262/#sec-array.prototype-@@unscopables

var includes = entryVirtual('Array').includes;

// https://tc39.github.io/ecma262/#sec-string.prototype.includes


_export({
  target: 'String',
  proto: true,
  forced: !correctIsRegexpLogic('includes')
}, {
  includes: function includes(searchString
  /* , position = 0 */
  ) {
    return !!~String(requireObjectCoercible(this)).indexOf(notARegexp(searchString), arguments.length > 1 ? arguments[1] : undefined);
  }
});

var includes$1 = entryVirtual('String').includes;

var ArrayPrototype$7 = Array.prototype;
var StringPrototype$1 = String.prototype;

var includes$2 = function (it) {
  var own = it.includes;
  if (it === ArrayPrototype$7 || it instanceof Array && own === ArrayPrototype$7.includes) return includes;

  if (typeof it === 'string' || it === StringPrototype$1 || it instanceof String && own === StringPrototype$1.includes) {
    return includes$1;
  }

  return own;
};

var includes$3 = includes$2;

var includes$4 = includes$3;

var $map = arrayIteration.map;
var HAS_SPECIES_SUPPORT$1 = arrayMethodHasSpeciesSupport('map'); // FF49- issue

var USES_TO_LENGTH$1 = HAS_SPECIES_SUPPORT$1 && !fails(function () {
  [].map.call({
    length: -1,
    0: 1
  }, function (it) {
    throw it;
  });
}); // `Array.prototype.map` method
// https://tc39.github.io/ecma262/#sec-array.prototype.map
// with adding support of @@species

_export({
  target: 'Array',
  proto: true,
  forced: !HAS_SPECIES_SUPPORT$1 || !USES_TO_LENGTH$1
}, {
  map: function map(callbackfn
  /* , thisArg */
  ) {
    return $map(this, callbackfn, arguments.length > 1 ? arguments[1] : undefined);
  }
});

var map$3 = entryVirtual('Array').map;

var ArrayPrototype$8 = Array.prototype;

var map_1 = function (it) {
  var own = it.map;
  return it === ArrayPrototype$8 || it instanceof Array && own === ArrayPrototype$8.map ? map$3 : own;
};

var map$4 = map_1;

var map$5 = map$4;

var createMethod$3 = function (IS_RIGHT) {
  return function (that, callbackfn, argumentsLength, memo) {
    aFunction(callbackfn);
    var O = toObject(that);
    var self = indexedObject(O);
    var length = toLength(O.length);
    var index = IS_RIGHT ? length - 1 : 0;
    var i = IS_RIGHT ? -1 : 1;
    if (argumentsLength < 2) while (true) {
      if (index in self) {
        memo = self[index];
        index += i;
        break;
      }

      index += i;

      if (IS_RIGHT ? index < 0 : length <= index) {
        throw TypeError('Reduce of empty array with no initial value');
      }
    }

    for (; IS_RIGHT ? index >= 0 : length > index; index += i) if (index in self) {
      memo = callbackfn(memo, self[index], index, O);
    }

    return memo;
  };
};

var arrayReduce = {
  // `Array.prototype.reduce` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.reduce
  left: createMethod$3(false),
  // `Array.prototype.reduceRight` method
  // https://tc39.github.io/ecma262/#sec-array.prototype.reduceright
  right: createMethod$3(true)
};

var sloppyArrayMethod = function (METHOD_NAME, argument) {
  var method = [][METHOD_NAME];
  return !method || !fails(function () {
    // eslint-disable-next-line no-useless-call,no-throw-literal
    method.call(null, argument || function () {
      throw 1;
    }, 1);
  });
};

var $reduce = arrayReduce.left; // `Array.prototype.reduce` method
// https://tc39.github.io/ecma262/#sec-array.prototype.reduce

_export({
  target: 'Array',
  proto: true,
  forced: sloppyArrayMethod('reduce')
}, {
  reduce: function reduce(callbackfn
  /* , initialValue */
  ) {
    return $reduce(this, callbackfn, arguments.length, arguments.length > 1 ? arguments[1] : undefined);
  }
});

var reduce = entryVirtual('Array').reduce;

var ArrayPrototype$9 = Array.prototype;

var reduce_1 = function (it) {
  var own = it.reduce;
  return it === ArrayPrototype$9 || it instanceof Array && own === ArrayPrototype$9.reduce ? reduce : own;
};

var reduce$1 = reduce_1;

var reduce$2 = reduce$1;

var test$1 = [];
var nativeSort = test$1.sort; // IE8-

var FAILS_ON_UNDEFINED = fails(function () {
  test$1.sort(undefined);
}); // V8 bug

var FAILS_ON_NULL = fails(function () {
  test$1.sort(null);
}); // Old WebKit

var SLOPPY_METHOD = sloppyArrayMethod('sort');
var FORCED$2 = FAILS_ON_UNDEFINED || !FAILS_ON_NULL || SLOPPY_METHOD; // `Array.prototype.sort` method
// https://tc39.github.io/ecma262/#sec-array.prototype.sort

_export({
  target: 'Array',
  proto: true,
  forced: FORCED$2
}, {
  sort: function sort(comparefn) {
    return comparefn === undefined ? nativeSort.call(toObject(this)) : nativeSort.call(toObject(this), aFunction(comparefn));
  }
});

var sort = entryVirtual('Array').sort;

var ArrayPrototype$a = Array.prototype;

var sort_1 = function (it) {
  var own = it.sort;
  return it === ArrayPrototype$a || it instanceof Array && own === ArrayPrototype$a.sort ? sort : own;
};

var sort$1 = sort_1;

var sort$2 = sort$1;

var min$3 = Math.min;
var nativeLastIndexOf = [].lastIndexOf;
var NEGATIVE_ZERO = !!nativeLastIndexOf && 1 / [1].lastIndexOf(1, -0) < 0;
var SLOPPY_METHOD$1 = sloppyArrayMethod('lastIndexOf'); // `Array.prototype.lastIndexOf` method implementation
// https://tc39.github.io/ecma262/#sec-array.prototype.lastindexof

var arrayLastIndexOf = NEGATIVE_ZERO || SLOPPY_METHOD$1 ? function lastIndexOf(searchElement
/* , fromIndex = @[*-1] */
) {
  // convert -0 to +0
  if (NEGATIVE_ZERO) return nativeLastIndexOf.apply(this, arguments) || 0;
  var O = toIndexedObject(this);
  var length = toLength(O.length);
  var index = length - 1;
  if (arguments.length > 1) index = min$3(index, toInteger(arguments[1]));
  if (index < 0) index = length + index;

  for (; index >= 0; index--) if (index in O && O[index] === searchElement) return index || 0;

  return -1;
} : nativeLastIndexOf;

// https://tc39.github.io/ecma262/#sec-array.prototype.lastindexof

_export({
  target: 'Array',
  proto: true,
  forced: arrayLastIndexOf !== [].lastIndexOf
}, {
  lastIndexOf: arrayLastIndexOf
});

var lastIndexOf = entryVirtual('Array').lastIndexOf;

var ArrayPrototype$b = Array.prototype;

var lastIndexOf_1 = function (it) {
  var own = it.lastIndexOf;
  return it === ArrayPrototype$b || it instanceof Array && own === ArrayPrototype$b.lastIndexOf ? lastIndexOf : own;
};

var lastIndexOf$1 = lastIndexOf_1;

var lastIndexOf$2 = lastIndexOf$1;

var $forEach$1 = arrayIteration.forEach; // `Array.prototype.forEach` method implementation
// https://tc39.github.io/ecma262/#sec-array.prototype.foreach

var arrayForEach = sloppyArrayMethod('forEach') ? function forEach(callbackfn
/* , thisArg */
) {
  return $forEach$1(this, callbackfn, arguments.length > 1 ? arguments[1] : undefined);
} : [].forEach;

// https://tc39.github.io/ecma262/#sec-array.prototype.foreach


_export({
  target: 'Array',
  proto: true,
  forced: [].forEach != arrayForEach
}, {
  forEach: arrayForEach
});

var forEach$1 = entryVirtual('Array').forEach;

var forEach$2 = forEach$1;

var ArrayPrototype$c = Array.prototype;
var DOMIterables$2 = {
  DOMTokenList: true,
  NodeList: true
};

var forEach_1 = function (it) {
  var own = it.forEach;
  return it === ArrayPrototype$c || it instanceof Array && own === ArrayPrototype$c.forEach // eslint-disable-next-line no-prototype-builtins
  || DOMIterables$2.hasOwnProperty(classof(it)) ? forEach$2 : own;
};

var forEach$3 = forEach_1;

var RouteStartupModule =
/*#__PURE__*/
function (_BlocksModule) {
  _inherits(RouteStartupModule, _BlocksModule);

  function RouteStartupModule() {
    var _this;

    _classCallCheck(this, RouteStartupModule);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(RouteStartupModule).call(this));
    _this.moduleName = "RouteStartupModule";
    return _this;
  }

  _createClass(RouteStartupModule, [{
    key: "preInitialize",
    value: function preInitialize() {
      var _context;

      var iocManagerTmp;
      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function preInitialize$(_context2) {
        while (1) {
          switch (_context2.prev = _context2.next) {
            case 0:
              if (this.shell) {
                _context2.next = 2;
                break;
              }

              throw new Error("Shell is null or empty.");

            case 2:
              if (this.iocManager) {
                _context2.next = 4;
                break;
              }

              throw new Error("iocManager is null or empty.");

            case 4:
              iocManagerTmp = this.iocManager;
              iocManagerTmp.register(function (c) {
                //hahhahaahahah
                debugger;

                bind$2(c).call(c, Types.IRouteManager).to(DefaultRouteManager).inSingletonScope();

                decorateIfNoExist(inversify_8(), DefaultRouteManager);
              });

              forEach$3(_context = this.shell.moduleMapTypes).call(_context, function (typeMap, ModuleType) {
                forEach$3(typeMap).call(typeMap, function (type, index) {
                  if (type.prototype instanceof IRouteProvider) {
                    iocManagerTmp.register(function (c) {
                      bind$2(c).call(c, Types.IRouteProvider).to(type).inTransientScope();
                    });
                  }

                  if (type.prototype instanceof ITemplateProvider) {
                    iocManagerTmp.register(function (c) {
                      bind$2(c).call(c, Types.ITemplateProvider).to(type).inTransientScope();
                    });
                  }
                });
              });

            case 7:
            case "end":
              return _context2.stop();
          }
        }
      }, null, this);
    }
  }]);

  return RouteStartupModule;
}(BlocksModule);

__decorate([inversify_11(Types.IBlocksShell), __metadata("design:type", IShell)], RouteStartupModule.prototype, "shell", void 0);

var resourceManager =
/*#__PURE__*/
function () {
  function resourceManager(blocksShell, manifestProviders) {
    _classCallCheck(this, resourceManager);

    this.resources = new map$2();
    this.blocksShell = blocksShell;
    this.manifestProviders = manifestProviders;
  }

  _createClass(resourceManager, [{
    key: "getResources",
    value: function getResources() {
      if (this.resources.size === 0) this.buildResource();
      return this.resources;
    }
  }, {
    key: "getResource",
    value: function getResource(moduleName, resourceType, resourceName) {
      var moduleResource = this.getResources().get(moduleName);

      if (!moduleResource || !moduleResource.has(resourceManager.getResourceKey(resourceType, resourceName))) {
        var _context;

        throw new Error(concat$2(_context = "Can not found resourceType:".concat(resourceType, ", resourceName:")).call(_context, resourceName, " in resources."));
      }

      return moduleResource.get(resourceManager.getResourceKey(resourceType, resourceName));
    }
  }, {
    key: "buildResource",
    value: function buildResource() {
      var _iteratorNormalCompletion = true;
      var _didIteratorError = false;
      var _iteratorError = undefined;

      try {
        for (var _iterator = getIterator$1(this.manifestProviders), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
          var manifestProvider = _step.value;
          var builder = new ResourceManifestBuilder();
          manifestProvider.buildManifests(builder);
          var moduleName = this.blocksShell.typeMapModuleName.get(manifestProvider.constructor);
          if (moduleName === undefined) throw new Error("".concat(manifestProvider.constructor, " can not found moduleName."));
          if (!this.resources.has(moduleName)) this.resources.set(moduleName, new map$2());
          var _iteratorNormalCompletion2 = true;
          var _didIteratorError2 = false;
          var _iteratorError2 = undefined;

          try {
            for (var _iterator2 = getIterator$1(builder.resourceManifests), _step2; !(_iteratorNormalCompletion2 = (_step2 = _iterator2.next()).done); _iteratorNormalCompletion2 = true) {
              var manifest = _step2.value;
              var _iteratorNormalCompletion3 = true;
              var _didIteratorError3 = false;
              var _iteratorError3 = undefined;

              try {
                for (var _iterator3 = getIterator$1(manifest.resources), _step3; !(_iteratorNormalCompletion3 = (_step3 = _iterator3.next()).done); _iteratorNormalCompletion3 = true) {
                  var resource = _step3.value;
                  var resourceNameMap = this.resources.get(moduleName);
                  if (resourceNameMap) resourceNameMap.set(resourceManager.getResourceKey(resource.resourceType, resource.resourceName), resource);
                }
              } catch (err) {
                _didIteratorError3 = true;
                _iteratorError3 = err;
              } finally {
                try {
                  if (!_iteratorNormalCompletion3 && _iterator3.return != null) {
                    _iterator3.return();
                  }
                } finally {
                  if (_didIteratorError3) {
                    throw _iteratorError3;
                  }
                }
              }
            }
          } catch (err) {
            _didIteratorError2 = true;
            _iteratorError2 = err;
          } finally {
            try {
              if (!_iteratorNormalCompletion2 && _iterator2.return != null) {
                _iterator2.return();
              }
            } finally {
              if (_didIteratorError2) {
                throw _iteratorError2;
              }
            }
          }
        }
      } catch (err) {
        _didIteratorError = true;
        _iteratorError = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion && _iterator.return != null) {
            _iterator.return();
          }
        } finally {
          if (_didIteratorError) {
            throw _iteratorError;
          }
        }
      }
    }
  }]);

  return resourceManager;
}();

resourceManager.getResourceKey = function (resourceType, resourceName) {
  return resourceType + "-" + resourceName;
};

resourceManager = __decorate([__param(0, inversify_11(Types.IBlocksShell)), __param(1, inversify_15(Types.IManifestProvider)), __metadata("design:paramtypes", [IBlocksShell, Array])], resourceManager);

var manifestStartupModule =
/*#__PURE__*/
function (_BlocksModule) {
  _inherits(manifestStartupModule, _BlocksModule);

  function manifestStartupModule() {
    var _this;

    _classCallCheck(this, manifestStartupModule);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(manifestStartupModule).call(this));
    _this.moduleName = "ManifestStartupModule";
    return _this;
  }

  _createClass(manifestStartupModule, [{
    key: "preInitialize",
    value: function preInitialize() {
      var _context;

      var iocManagerTmp;
      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function preInitialize$(_context2) {
        while (1) {
          switch (_context2.prev = _context2.next) {
            case 0:
              if (this.shell) {
                _context2.next = 2;
                break;
              }

              throw new Error("Shell is null or empty.");

            case 2:
              if (this.iocManager) {
                _context2.next = 4;
                break;
              }

              throw new Error("iocManager is null or empty.");

            case 4:
              iocManagerTmp = this.iocManager;
              iocManagerTmp.register(function (c) {
                bind$2(c).call(c, Types.IResourceManager).to(resourceManager).inSingletonScope();

                decorateIfNoExist(inversify_8(), resourceManager);
              });

              forEach$3(_context = this.shell.moduleMapTypes).call(_context, function (typeMap, ModuleType) {
                forEach$3(typeMap).call(typeMap, function (type, index) {
                  if (type.prototype instanceof IManifestProvider) {
                    iocManagerTmp.register(function (c) {
                      bind$2(c).call(c, Types.IManifestProvider).to(type).inTransientScope();
                    });
                  }
                });
              });

            case 7:
            case "end":
              return _context2.stop();
          }
        }
      }, null, this);
    }
  }]);

  return manifestStartupModule;
}(BlocksModule);

__decorate([inversify_11(Types.IBlocksShell), __metadata("design:type", IShell)], manifestStartupModule.prototype, "shell", void 0);

var BlocksShell =
/*#__PURE__*/
function () {
  function BlocksShell(iocManager) {
    _classCallCheck(this, BlocksShell);

    this.pluginSource = [];
    this.iocManager = iocManager;
    this.types = [];
    this.typeMapModuleName = new map$2();
    this.moduleMapTypes = new map$2();
    this.typeMapFileName = new map$2();
    this.blocksModules = [];
  }

  _createClass(BlocksShell, [{
    key: "initialize",
    value: function initialize() {
      var _context,
          _this = this,
          _context4,
          _context5;

      var _iteratorNormalCompletion, _didIteratorError, _iteratorError, _iterator, _step, filesArray, startupModules, temp, _iteratorNormalCompletion2, _didIteratorError2, _iteratorError2, _iterator2, _step2, module, _iteratorNormalCompletion3, _didIteratorError3, _iteratorError3, _iterator3, _step3, startupModule, moduleType, _iteratorNormalCompletion6, _didIteratorError6, _iteratorError6, _iterator6, _step6, type, orders, orderGroup, _iteratorNormalCompletion4, _didIteratorError4, _iteratorError4, _iterator4, _step4, orderModules, _iteratorNormalCompletion5, _didIteratorError5, _iteratorError5, _iterator5, _step5, _orderModules;

      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function initialize$(_context6) {
        while (1) {
          switch (_context6.prev = _context6.next) {
            case 0:
              _iteratorNormalCompletion = true;
              _didIteratorError = false;
              _iteratorError = undefined;
              _context6.prev = 3;

              for (_iterator = getIterator$1(this.getTypeFromFiles()); !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
                filesArray = _step.value;
                this.moduleAndDependencyLoader(filesArray);
              }

              _context6.next = 11;
              break;

            case 7:
              _context6.prev = 7;
              _context6.t0 = _context6["catch"](3);
              _didIteratorError = true;
              _iteratorError = _context6.t0;

            case 11:
              _context6.prev = 11;
              _context6.prev = 12;

              if (!_iteratorNormalCompletion && _iterator.return != null) {
                _iterator.return();
              }

            case 14:
              _context6.prev = 14;

              if (!_didIteratorError) {
                _context6.next = 17;
                break;
              }

              throw _iteratorError;

            case 17:
              return _context6.finish(14);

            case 18:
              return _context6.finish(11);

            case 19:
              this.autoRegisterModule(RouteStartupModule);
              this.autoRegisterModule(manifestStartupModule);
              startupModules = this.iocManager.getAll(BlocksModule);
              this.blocksModules = startupModules; //

              temp = new map$2();
              _iteratorNormalCompletion2 = true;
              _didIteratorError2 = false;
              _iteratorError2 = undefined;
              _context6.prev = 27;
              _iterator2 = getIterator$1(startupModules);

            case 29:
              if (_iteratorNormalCompletion2 = (_step2 = _iterator2.next()).done) {
                _context6.next = 37;
                break;
              }

              module = _step2.value;

              if (!temp.has(module.moduleName)) {
                _context6.next = 33;
                break;
              }

              throw new Error("ModuleName ".concat(module.moduleName, " is registered double."));

            case 33:
              temp.set(module.moduleName, []);

            case 34:
              _iteratorNormalCompletion2 = true;
              _context6.next = 29;
              break;

            case 37:
              _context6.next = 43;
              break;

            case 39:
              _context6.prev = 39;
              _context6.t1 = _context6["catch"](27);
              _didIteratorError2 = true;
              _iteratorError2 = _context6.t1;

            case 43:
              _context6.prev = 43;
              _context6.prev = 44;

              if (!_iteratorNormalCompletion2 && _iterator2.return != null) {
                _iterator2.return();
              }

            case 46:
              _context6.prev = 46;

              if (!_didIteratorError2) {
                _context6.next = 49;
                break;
              }

              throw _iteratorError2;

            case 49:
              return _context6.finish(46);

            case 50:
              return _context6.finish(43);

            case 51:
              // for (let module of startupModules) {
              //     if (this.ModuleMapTypes.has(module.ModuleName))
              //         throw new Error(`ModuleName {ModuleName} is registered double.`)
              //     this.ModuleMapTypes.set(module.ModuleName, []);
              // }
              forEach$3(_context = this.moduleMapTypes).call(_context, function (moduleMapSet, moduleTypeKey) {
                var _context2;

                var moduleType = _this.typeMapFileName.get(moduleTypeKey);

                if (!moduleType) return;
                var moduleFilePrefix = moduleType[1];
                moduleFilePrefix = moduleFilePrefix.substring(0, lastIndexOf$2(moduleFilePrefix).call(moduleFilePrefix, '/') + 1);

                forEach$3(_context2 = _this.typeMapFileName).call(_context2, function (value, typeMapKey) {
                  var _context3;

                  if (startsWith$2(_context3 = value[1]).call(_context3, moduleFilePrefix)) {
                    moduleMapSet.push(typeMapKey);
                  }
                });
              });

              _iteratorNormalCompletion3 = true;
              _didIteratorError3 = false;
              _iteratorError3 = undefined;
              _context6.prev = 55;
              _iterator3 = getIterator$1(startupModules);

            case 57:
              if (_iteratorNormalCompletion3 = (_step3 = _iterator3.next()).done) {
                _context6.next = 86;
                break;
              }

              startupModule = _step3.value;

              if (this.moduleMapTypes.has(startupModule.constructor)) {
                _context6.next = 61;
                break;
              }

              return _context6.abrupt("continue", 83);

            case 61:
              moduleType = this.moduleMapTypes.get(startupModule.constructor);

              if (moduleType) {
                _context6.next = 64;
                break;
              }

              return _context6.abrupt("continue", 83);

            case 64:
              _iteratorNormalCompletion6 = true;
              _didIteratorError6 = false;
              _iteratorError6 = undefined;
              _context6.prev = 67;

              for (_iterator6 = getIterator$1(moduleType); !(_iteratorNormalCompletion6 = (_step6 = _iterator6.next()).done); _iteratorNormalCompletion6 = true) {
                type = _step6.value;
                this.typeMapModuleName.set(type, startupModule.moduleName);
              }

              _context6.next = 75;
              break;

            case 71:
              _context6.prev = 71;
              _context6.t2 = _context6["catch"](67);
              _didIteratorError6 = true;
              _iteratorError6 = _context6.t2;

            case 75:
              _context6.prev = 75;
              _context6.prev = 76;

              if (!_iteratorNormalCompletion6 && _iterator6.return != null) {
                _iterator6.return();
              }

            case 78:
              _context6.prev = 78;

              if (!_didIteratorError6) {
                _context6.next = 81;
                break;
              }

              throw _iteratorError6;

            case 81:
              return _context6.finish(78);

            case 82:
              return _context6.finish(75);

            case 83:
              _iteratorNormalCompletion3 = true;
              _context6.next = 57;
              break;

            case 86:
              _context6.next = 92;
              break;

            case 88:
              _context6.prev = 88;
              _context6.t3 = _context6["catch"](55);
              _didIteratorError3 = true;
              _iteratorError3 = _context6.t3;

            case 92:
              _context6.prev = 92;
              _context6.prev = 93;

              if (!_iteratorNormalCompletion3 && _iterator3.return != null) {
                _iterator3.return();
              }

            case 95:
              _context6.prev = 95;

              if (!_didIteratorError3) {
                _context6.next = 98;
                break;
              }

              throw _iteratorError3;

            case 98:
              return _context6.finish(95);

            case 99:
              return _context6.finish(92);

            case 100:
              orders = sort$2(_context4 = reduce$2(_context5 = map$5(startupModules).call(startupModules, function (m) {
                return m.order;
              })).call(_context5, function (cur, next) {
                if (!includes$4(cur).call(cur, next)) cur.push(next);
                return cur;
              }, new Array())).call(_context4, function (a, b) {
                return a - b;
              });
              orderGroup = reduce$2(orders).call(orders, function (cur, next) {
                cur.push(filter$2(startupModules).call(startupModules, function (m) {
                  return m.order === next;
                }));
                return cur;
              }, new Array());
              _iteratorNormalCompletion4 = true;
              _didIteratorError4 = false;
              _iteratorError4 = undefined;
              _context6.prev = 105;
              _iterator4 = getIterator$1(orderGroup);

            case 107:
              if (_iteratorNormalCompletion4 = (_step4 = _iterator4.next()).done) {
                _context6.next = 114;
                break;
              }

              orderModules = _step4.value;
              _context6.next = 111;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(promise$3.all(map$5(orderModules).call(orderModules, function (m) {
                return m.preInitialize();
              })));

            case 111:
              _iteratorNormalCompletion4 = true;
              _context6.next = 107;
              break;

            case 114:
              _context6.next = 120;
              break;

            case 116:
              _context6.prev = 116;
              _context6.t4 = _context6["catch"](105);
              _didIteratorError4 = true;
              _iteratorError4 = _context6.t4;

            case 120:
              _context6.prev = 120;
              _context6.prev = 121;

              if (!_iteratorNormalCompletion4 && _iterator4.return != null) {
                _iterator4.return();
              }

            case 123:
              _context6.prev = 123;

              if (!_didIteratorError4) {
                _context6.next = 126;
                break;
              }

              throw _iteratorError4;

            case 126:
              return _context6.finish(123);

            case 127:
              return _context6.finish(120);

            case 128:
              _iteratorNormalCompletion5 = true;
              _didIteratorError5 = false;
              _iteratorError5 = undefined;
              _context6.prev = 131;
              _iterator5 = getIterator$1(orderGroup);

            case 133:
              if (_iteratorNormalCompletion5 = (_step5 = _iterator5.next()).done) {
                _context6.next = 140;
                break;
              }

              _orderModules = _step5.value;
              _context6.next = 137;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(promise$3.all(map$5(_orderModules).call(_orderModules, function (m) {
                return m.initialize();
              })));

            case 137:
              _iteratorNormalCompletion5 = true;
              _context6.next = 133;
              break;

            case 140:
              _context6.next = 146;
              break;

            case 142:
              _context6.prev = 142;
              _context6.t5 = _context6["catch"](131);
              _didIteratorError5 = true;
              _iteratorError5 = _context6.t5;

            case 146:
              _context6.prev = 146;
              _context6.prev = 147;

              if (!_iteratorNormalCompletion5 && _iterator5.return != null) {
                _iterator5.return();
              }

            case 149:
              _context6.prev = 149;

              if (!_didIteratorError5) {
                _context6.next = 152;
                break;
              }

              throw _iteratorError5;

            case 152:
              return _context6.finish(149);

            case 153:
              return _context6.finish(146);

            case 154:
            case "end":
              return _context6.stop();
          }
        }
      }, null, this, [[3, 7, 11, 19], [12,, 14, 18], [27, 39, 43, 51], [44,, 46, 50], [55, 88, 92, 100], [67, 71, 75, 83], [76,, 78, 82], [93,, 95, 99], [105, 116, 120, 128], [121,, 123, 127], [131, 142, 146, 154], [147,, 149, 153]]);
    }
  }, {
    key: "moduleAndDependencyLoader",
    value: function moduleAndDependencyLoader(filesArray) {
      var _this2 = this;

      var registerAction = function registerAction(fileKey) {
        var exportTypes = filesArray(fileKey);

        for (var _i = 0, _Object$keys = keys$6(exportTypes); _i < _Object$keys.length; _i++) {
          var objKey = _Object$keys[_i];
          var exportType = exportTypes[objKey];
          if (!exportType) continue;

          var isModule = _this2.autoRegisterModule(exportType);

          if (isModule) _this2.moduleMapTypes.set(exportType, []);

          if (exportType.prototype instanceof IDependency) {
            _this2.typeMapFileName.set(exportType, [isModule, fileKey]);

            _this2.types.push(exportTypes[objKey]);
          }

          if (exportType.prototype instanceof Controller && exportType.options.type) {
            _this2.typeMapFileName.set(exportType.options.type, [isModule, fileKey]);

            _this2.types.push(exportType.options.type);
          } // if(fileKey.lastIndexOf(".bl.ts"))
          // {
          //     debugger
          //     decorate(Component(), exportType);
          // }

        }
      };

      var _iteratorNormalCompletion7 = true;
      var _didIteratorError7 = false;
      var _iteratorError7 = undefined;

      try {
        for (var _iterator7 = getIterator$1(keys$3(filesArray).call(filesArray)), _step7; !(_iteratorNormalCompletion7 = (_step7 = _iterator7.next()).done); _iteratorNormalCompletion7 = true) {
          var fileKey = _step7.value;
          registerAction(fileKey);
        }
      } catch (err) {
        _didIteratorError7 = true;
        _iteratorError7 = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion7 && _iterator7.return != null) {
            _iterator7.return();
          }
        } finally {
          if (_didIteratorError7) {
            throw _iteratorError7;
          }
        }
      }
    }
  }, {
    key: "autoRegisterModule",
    value: function autoRegisterModule(type) {
      if (type.prototype instanceof BlocksModule) {
        this.iocManager.register(function (c) {
          bind$2(c).call(c, BlocksModule).to(type).inTransientScope();

          decorateIfNoExist(inversify_8(), type);
        });
        return true;
      }

      return false;
    }
  }, {
    key: "getTypeFromFiles",
    value: function getTypeFromFiles() {
      var _context7;

      return values$2(_context7 = this.pluginSource).call(_context7);
    }
  }, {
    key: "BlocksModules",
    get: function get() {
      return this.blocksModules;
    }
  }]);

  return BlocksShell;
}();

BlocksShell = __decorate([__param(0, inversify_11(IocManager)), __metadata("design:paramtypes", [IocManager])], BlocksShell);

var BlocksBoostrapper =
/*#__PURE__*/
function (_IBootstrapper) {
  _inherits(BlocksBoostrapper, _IBootstrapper);

  function BlocksBoostrapper(startModule, optionsFunc) {
    var _this;

    _classCallCheck(this, BlocksBoostrapper);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(BlocksBoostrapper).call(this));
    var bootstarpperOptions = new BootstrapperOptions();

    if (optionsFunc !== undefined) {
      optionsFunc(bootstarpperOptions);
    }

    _this.iocManager = bootstarpperOptions.iocManager;
    _this.startModule = startModule;
    _this.plugInSources = []; // this.RouteHelper = new RouteHelperCls(this.iocManager);

    return _this;
  }

  _createClass(BlocksBoostrapper, [{
    key: "initialize",
    value: function initialize() {
      var shell;
      return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.async(function initialize$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              //Logger
              this.registerShell();
              shell = this.iocManager.get(Types.IBlocksShell);
              shell.pluginSource = this.plugInSources;
              _context.next = 5;
              return C__D_Repository_BlocksModuleLastest_Lastest_ModuleLoader_node_modules__babel_runtimeCorejs3_regenerator.awrap(shell.initialize());

            case 5:
            case "end":
              return _context.stop();
          }
        }
      }, null, this);
    }
  }, {
    key: "registerShell",
    value: function registerShell() {
      var _this2 = this;

      this.iocManager.register(function (c) {
        bind$2(c).call(c, IocManager).toConstantValue(_this2.iocManager);

        bind$2(c).call(c, Types.IBlocksShell).to(BlocksShell).inSingletonScope(); //c.bind<BlocksShell>(Types.IBlocksShell).to(BlocksShell).inSingletonScope();


        decorateIfNoExist(inversify_8(), IocManager);
        decorateIfNoExist(inversify_8(), BlocksShell);
      });
    }
  }, {
    key: "dispose",
    value: function dispose() {
      this.iocManager.unbindAll();
    }
  }, {
    key: "RouteHelper",
    get: function get() {
      return this.iocManager.get(Types.IRouteManager);
    }
  }, {
    key: "PlugInSources",
    get: function get() {
      return this.plugInSources;
    }
  }], [{
    key: "create",
    value: function create(startModule, optionsFunc) {
      return new BlocksBoostrapper(startModule, optionsFunc);
    }
  }]);

  return BlocksBoostrapper;
}(IBootstrapper);

/**
 * @param { Promise } promise
 * @param { Object= } errorExt - Additional Information you can pass to the err object
 * @return { Promise }
 */
function to(promise, errorExt) {
  return promise.then(function (data) {
    return [null, data];
  }).catch(function (err) {
    if (errorExt) {
      Object.assign(err, errorExt);
    }

    return [err, undefined];
  });
}

var TYPES = {
  BlocksModule: _for$2("BlocksModule")
};
inversify_20(inversify_8(), BlocksModule);
var Bootstrapper = BlocksBoostrapper.create(undefined, function (o) {
  return o.iocManager = globalIocManager;
});

export { BlocksModule, Bootstrapper, Component, Controller, IBlocksShell, IBootstrapper, IDependency, IManifestProvider, IRouteProvider, IShell, ITemplateProvider, IocManager, RouteResult, TYPES, TemplateResult, Types, asyncCompatible, to as catchWrap, decorateIfNoExist, globalIocManager, inversify_11 as inject, inversify_8 as injectable };
