#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <stdint.h>
#include <limits>



// System.Action`1<UnityEngineInternal.Input.NativeInputUpdateType>
struct Action_1_t7797D4D8783204B10C3D28B96B049C48276C3B1B;
// System.Action`1<UnityEngine.Playables.PlayableDirector>
struct Action_1_tB645F646DB079054A9500B09427CB02A88372D3F;
// System.Action`1<System.String>
struct Action_1_t3CB5D1A819C3ED3F99E9E39F890F18633253949A;
// System.Action`2<System.Int32,System.String>
struct Action_2_t6AAF2E215E74E16A4EEF0A0749A4A325D99F5BA6;
// System.Func`2<UnityEngineInternal.Input.NativeInputUpdateType,System.Boolean>
struct Func_2_t880CA675AE5D39E081BEEF14DC092D82674DE4F2;
// System.Collections.Generic.List`1<UnityEngine.IntegratedSubsystem>
struct List_1_t78E7232867D713AA9907E71F6C5B19B226F0B180;
// System.Collections.Generic.List`1<UnityEngine.IntegratedSubsystemDescriptor>
struct List_1_tACFC79734710927A89702FFC38900223BB85B5A6;
// System.Collections.Generic.List`1<UnityEngine.Subsystem>
struct List_1_t9E8CCD70A25458CE30A64503B35F06ECA62E3052;
// System.Collections.Generic.List`1<UnityEngine.SubsystemDescriptor>
struct List_1_t15AD773D34D3739AFB67421B6DFFACEA7638F64E;
// System.Collections.Generic.List`1<UnityEngine.SubsystemsImplementation.SubsystemDescriptorWithProvider>
struct List_1_t2D19D6F759F401FE6C5460698E5B8249E470E044;
// System.Collections.Generic.List`1<UnityEngine.SubsystemsImplementation.SubsystemWithProvider>
struct List_1_tD834E8FB7FDC0D4243FBCF922D7FE4E3C707AAC3;
// System.String[]
struct StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248;
// System.Action
struct Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07;
// UnityEngine.ISubsystemDescriptor
struct ISubsystemDescriptor_tEF29944D579CC7D70F52CB883150735991D54E6E;
// UnityEngineInternal.Input.NativeUpdateCallback
struct NativeUpdateCallback_tC5CA5A9117B79251968A4DA3758552EFE1D37495;
// UnityEngine.ParticleSystem
struct ParticleSystem_tB19986EE308BD63D36FB6025EEEAFBEDB97C67C1;
// System.String
struct String_t;
// UnityEngine.Texture2D
struct Texture2D_tE6505BC111DD8A424A9DBE8E05D7D09E11FFFCF4;
// System.Void
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;



IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Attribute
struct Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA  : public RuntimeObject
{
};

// UnityEngineInternal.Input.NativeInputSystem
struct NativeInputSystem_tCFE5554EBC0D3EE1DAD80FC55CE0DE38A3DDC5EE  : public RuntimeObject
{
};

struct NativeInputSystem_tCFE5554EBC0D3EE1DAD80FC55CE0DE38A3DDC5EE_StaticFields
{
	// UnityEngineInternal.Input.NativeUpdateCallback UnityEngineInternal.Input.NativeInputSystem::onUpdate
	NativeUpdateCallback_tC5CA5A9117B79251968A4DA3758552EFE1D37495* ___onUpdate_0;
	// System.Action`1<UnityEngineInternal.Input.NativeInputUpdateType> UnityEngineInternal.Input.NativeInputSystem::onBeforeUpdate
	Action_1_t7797D4D8783204B10C3D28B96B049C48276C3B1B* ___onBeforeUpdate_1;
	// System.Func`2<UnityEngineInternal.Input.NativeInputUpdateType,System.Boolean> UnityEngineInternal.Input.NativeInputSystem::onShouldRunUpdate
	Func_2_t880CA675AE5D39E081BEEF14DC092D82674DE4F2* ___onShouldRunUpdate_2;
	// System.Action`2<System.Int32,System.String> UnityEngineInternal.Input.NativeInputSystem::s_OnDeviceDiscoveredCallback
	Action_2_t6AAF2E215E74E16A4EEF0A0749A4A325D99F5BA6* ___s_OnDeviceDiscoveredCallback_3;
};

// UnityEngine.SubsystemDescriptor
struct SubsystemDescriptor_tF417D2751C69A8B0DD86162EBCE55F84D3493A71  : public RuntimeObject
{
	// System.String UnityEngine.SubsystemDescriptor::<id>k__BackingField
	String_t* ___U3CidU3Ek__BackingField_0;
};

// UnityEngine.SubsystemsImplementation.SubsystemDescriptorStore
struct SubsystemDescriptorStore_tEF3761B84B8C25EA4B93F94A487551820B268250  : public RuntimeObject
{
};

struct SubsystemDescriptorStore_tEF3761B84B8C25EA4B93F94A487551820B268250_StaticFields
{
	// System.Collections.Generic.List`1<UnityEngine.IntegratedSubsystemDescriptor> UnityEngine.SubsystemsImplementation.SubsystemDescriptorStore::s_IntegratedDescriptors
	List_1_tACFC79734710927A89702FFC38900223BB85B5A6* ___s_IntegratedDescriptors_0;
	// System.Collections.Generic.List`1<UnityEngine.SubsystemsImplementation.SubsystemDescriptorWithProvider> UnityEngine.SubsystemsImplementation.SubsystemDescriptorStore::s_StandaloneDescriptors
	List_1_t2D19D6F759F401FE6C5460698E5B8249E470E044* ___s_StandaloneDescriptors_1;
	// System.Collections.Generic.List`1<UnityEngine.SubsystemDescriptor> UnityEngine.SubsystemsImplementation.SubsystemDescriptorStore::s_DeprecatedDescriptors
	List_1_t15AD773D34D3739AFB67421B6DFFACEA7638F64E* ___s_DeprecatedDescriptors_2;
};

// UnityEngine.SubsystemsImplementation.SubsystemDescriptorWithProvider
struct SubsystemDescriptorWithProvider_t2A61A2C951A4A179E898CF207726BF6B5AF474D5  : public RuntimeObject
{
	// System.String UnityEngine.SubsystemsImplementation.SubsystemDescriptorWithProvider::<id>k__BackingField
	String_t* ___U3CidU3Ek__BackingField_0;
};

// UnityEngine.SubsystemManager
struct SubsystemManager_t9A7261E4D0B53B996F04B8707D8E1C33AB65E824  : public RuntimeObject
{
};

struct SubsystemManager_t9A7261E4D0B53B996F04B8707D8E1C33AB65E824_StaticFields
{
	// System.Action UnityEngine.SubsystemManager::beforeReloadSubsystems
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___beforeReloadSubsystems_0;
	// System.Action UnityEngine.SubsystemManager::afterReloadSubsystems
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___afterReloadSubsystems_1;
	// System.Collections.Generic.List`1<UnityEngine.IntegratedSubsystem> UnityEngine.SubsystemManager::s_IntegratedSubsystems
	List_1_t78E7232867D713AA9907E71F6C5B19B226F0B180* ___s_IntegratedSubsystems_2;
	// System.Collections.Generic.List`1<UnityEngine.SubsystemsImplementation.SubsystemWithProvider> UnityEngine.SubsystemManager::s_StandaloneSubsystems
	List_1_tD834E8FB7FDC0D4243FBCF922D7FE4E3C707AAC3* ___s_StandaloneSubsystems_3;
	// System.Collections.Generic.List`1<UnityEngine.Subsystem> UnityEngine.SubsystemManager::s_DeprecatedSubsystems
	List_1_t9E8CCD70A25458CE30A64503B35F06ECA62E3052* ___s_DeprecatedSubsystems_4;
	// System.Action UnityEngine.SubsystemManager::reloadSubsytemsStarted
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___reloadSubsytemsStarted_5;
	// System.Action UnityEngine.SubsystemManager::reloadSubsytemsCompleted
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___reloadSubsytemsCompleted_6;
};

// System.ValueType
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
// Native definition for P/Invoke marshalling of System.ValueType
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.ValueType
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};

// UnityEngine.XR.XRDevice
struct XRDevice_tD076A68EFE413B3EEEEA362BE0364A488B58F194  : public RuntimeObject
{
};

struct XRDevice_tD076A68EFE413B3EEEEA362BE0364A488B58F194_StaticFields
{
	// System.Action`1<System.String> UnityEngine.XR.XRDevice::deviceLoaded
	Action_1_t3CB5D1A819C3ED3F99E9E39F890F18633253949A* ___deviceLoaded_0;
};

// Unity.Collections.NativeArray`1<System.Byte>
struct NativeArray_1_t81F55263465517B73C455D3400CF67B4BADD85CF 
{
	// System.Void* Unity.Collections.NativeArray`1::m_Buffer
	void* ___m_Buffer_0;
	// System.Int32 Unity.Collections.NativeArray`1::m_Length
	int32_t ___m_Length_1;
	// Unity.Collections.Allocator Unity.Collections.NativeArray`1::m_AllocatorLabel
	int32_t ___m_AllocatorLabel_2;
};

// UnityEngine.Color32
struct Color32_t73C5004937BF5BB8AD55323D51AAA40A898EF48B 
{
	union
	{
		#pragma pack(push, tp, 1)
		struct
		{
			// System.Int32 UnityEngine.Color32::rgba
			int32_t ___rgba_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			int32_t ___rgba_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			// System.Byte UnityEngine.Color32::r
			uint8_t ___r_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			uint8_t ___r_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___g_2_OffsetPadding[1];
			// System.Byte UnityEngine.Color32::g
			uint8_t ___g_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___g_2_OffsetPadding_forAlignmentOnly[1];
			uint8_t ___g_2_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___b_3_OffsetPadding[2];
			// System.Byte UnityEngine.Color32::b
			uint8_t ___b_3;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___b_3_OffsetPadding_forAlignmentOnly[2];
			uint8_t ___b_3_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___a_4_OffsetPadding[3];
			// System.Byte UnityEngine.Color32::a
			uint8_t ___a_4;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___a_4_OffsetPadding_forAlignmentOnly[3];
			uint8_t ___a_4_forAlignmentOnly;
		};
	};
};

// UnityEngine.Bindings.IgnoreAttribute
struct IgnoreAttribute_tAB3F6C4808BA16CD585D60A6353B3E0599DFCE4D  : public Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA
{
	// System.Boolean UnityEngine.Bindings.IgnoreAttribute::<DoesNotContributeToSize>k__BackingField
	bool ___U3CDoesNotContributeToSizeU3Ek__BackingField_0;
};

// System.IntPtr
struct IntPtr_t 
{
	// System.Void* System.IntPtr::m_value
	void* ___m_value_0;
};

struct IntPtr_t_StaticFields
{
	// System.IntPtr System.IntPtr::Zero
	intptr_t ___Zero_1;
};

// UnityEngineInternal.Input.NativeInputEventBuffer
struct NativeInputEventBuffer_t4EE5873AD7998E0E83C9F8585C338AB14C9101FD 
{
	union
	{
		struct
		{
			union
			{
				#pragma pack(push, tp, 1)
				struct
				{
					// System.Void* UnityEngineInternal.Input.NativeInputEventBuffer::eventBuffer
					void* ___eventBuffer_0;
				};
				#pragma pack(pop, tp)
				#pragma pack(push, tp, 1)
				struct
				{
					void* ___eventBuffer_0_forAlignmentOnly;
				};
				#pragma pack(pop, tp)
				#pragma pack(push, tp, 1)
				struct
				{
					char ___eventCount_1_OffsetPadding[8];
					// System.Int32 UnityEngineInternal.Input.NativeInputEventBuffer::eventCount
					int32_t ___eventCount_1;
				};
				#pragma pack(pop, tp)
				#pragma pack(push, tp, 1)
				struct
				{
					char ___eventCount_1_OffsetPadding_forAlignmentOnly[8];
					int32_t ___eventCount_1_forAlignmentOnly;
				};
				#pragma pack(pop, tp)
				#pragma pack(push, tp, 1)
				struct
				{
					char ___sizeInBytes_2_OffsetPadding[12];
					// System.Int32 UnityEngineInternal.Input.NativeInputEventBuffer::sizeInBytes
					int32_t ___sizeInBytes_2;
				};
				#pragma pack(pop, tp)
				#pragma pack(push, tp, 1)
				struct
				{
					char ___sizeInBytes_2_OffsetPadding_forAlignmentOnly[12];
					int32_t ___sizeInBytes_2_forAlignmentOnly;
				};
				#pragma pack(pop, tp)
				#pragma pack(push, tp, 1)
				struct
				{
					char ___capacityInBytes_3_OffsetPadding[16];
					// System.Int32 UnityEngineInternal.Input.NativeInputEventBuffer::capacityInBytes
					int32_t ___capacityInBytes_3;
				};
				#pragma pack(pop, tp)
				#pragma pack(push, tp, 1)
				struct
				{
					char ___capacityInBytes_3_OffsetPadding_forAlignmentOnly[16];
					int32_t ___capacityInBytes_3_forAlignmentOnly;
				};
				#pragma pack(pop, tp)
			};
		};
		uint8_t NativeInputEventBuffer_t4EE5873AD7998E0E83C9F8585C338AB14C9101FD__padding[20];
	};
};

// UnityEngine.Bindings.NativeThrowsAttribute
struct NativeThrowsAttribute_t211CE8D047A8D45676C9ED399D5AA3B4A2C3E625  : public Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA
{
	// System.Boolean UnityEngine.Bindings.NativeThrowsAttribute::<ThrowsException>k__BackingField
	bool ___U3CThrowsExceptionU3Ek__BackingField_0;
};

// UnityEngine.PropertyAttribute
struct PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0  : public Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA
{
};

// UnityEngine.Scripting.RequiredByNativeCodeAttribute
struct RequiredByNativeCodeAttribute_t86B11F2BA12BB463CE3258E64E16B43484014FCA  : public Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA
{
	// System.String UnityEngine.Scripting.RequiredByNativeCodeAttribute::<Name>k__BackingField
	String_t* ___U3CNameU3Ek__BackingField_0;
	// System.Boolean UnityEngine.Scripting.RequiredByNativeCodeAttribute::<Optional>k__BackingField
	bool ___U3COptionalU3Ek__BackingField_1;
	// System.Boolean UnityEngine.Scripting.RequiredByNativeCodeAttribute::<GenerateProxy>k__BackingField
	bool ___U3CGenerateProxyU3Ek__BackingField_2;
};

// UnityEngine.Bindings.StaticAccessorAttribute
struct StaticAccessorAttribute_tDE194716AED7A414D473DC570B2E0035A5CE130A  : public Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA
{
	// System.String UnityEngine.Bindings.StaticAccessorAttribute::<Name>k__BackingField
	String_t* ___U3CNameU3Ek__BackingField_0;
	// UnityEngine.Bindings.StaticAccessorType UnityEngine.Bindings.StaticAccessorAttribute::<Type>k__BackingField
	int32_t ___U3CTypeU3Ek__BackingField_1;
};

// UnityEngine.Scripting.UsedByNativeCodeAttribute
struct UsedByNativeCodeAttribute_t3FE9A7CDCC6A3A4122D8BF44F1D0A37BB38894C1  : public Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA
{
	// System.String UnityEngine.Scripting.UsedByNativeCodeAttribute::<Name>k__BackingField
	String_t* ___U3CNameU3Ek__BackingField_0;
};

// UnityEngine.Vector3
struct Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 
{
	// System.Single UnityEngine.Vector3::x
	float ___x_2;
	// System.Single UnityEngine.Vector3::y
	float ___y_3;
	// System.Single UnityEngine.Vector3::z
	float ___z_4;
};

struct Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_StaticFields
{
	// UnityEngine.Vector3 UnityEngine.Vector3::zeroVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___zeroVector_5;
	// UnityEngine.Vector3 UnityEngine.Vector3::oneVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___oneVector_6;
	// UnityEngine.Vector3 UnityEngine.Vector3::upVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___upVector_7;
	// UnityEngine.Vector3 UnityEngine.Vector3::downVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___downVector_8;
	// UnityEngine.Vector3 UnityEngine.Vector3::leftVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___leftVector_9;
	// UnityEngine.Vector3 UnityEngine.Vector3::rightVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___rightVector_10;
	// UnityEngine.Vector3 UnityEngine.Vector3::forwardVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___forwardVector_11;
	// UnityEngine.Vector3 UnityEngine.Vector3::backVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___backVector_12;
	// UnityEngine.Vector3 UnityEngine.Vector3::positiveInfinityVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___positiveInfinityVector_13;
	// UnityEngine.Vector3 UnityEngine.Vector3::negativeInfinityVector
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___negativeInfinityVector_14;
};

// UnityEngine.ParticleSystem/MainModule
struct MainModule_tC7ECD8330C14B0808478A748048988A6085CE2A9 
{
	// UnityEngine.ParticleSystem UnityEngine.ParticleSystem/MainModule::m_ParticleSystem
	ParticleSystem_tB19986EE308BD63D36FB6025EEEAFBEDB97C67C1* ___m_ParticleSystem_0;
};
// Native definition for P/Invoke marshalling of UnityEngine.ParticleSystem/MainModule
struct MainModule_tC7ECD8330C14B0808478A748048988A6085CE2A9_marshaled_pinvoke
{
	ParticleSystem_tB19986EE308BD63D36FB6025EEEAFBEDB97C67C1* ___m_ParticleSystem_0;
};
// Native definition for COM marshalling of UnityEngine.ParticleSystem/MainModule
struct MainModule_tC7ECD8330C14B0808478A748048988A6085CE2A9_marshaled_com
{
	ParticleSystem_tB19986EE308BD63D36FB6025EEEAFBEDB97C67C1* ___m_ParticleSystem_0;
};

// UnityEngine.ParticleSystem/SubEmittersModule
struct SubEmittersModule_t94F5AD231EAFB50A16E697186A630B07BF8B949B 
{
	// UnityEngine.ParticleSystem UnityEngine.ParticleSystem/SubEmittersModule::m_ParticleSystem
	ParticleSystem_tB19986EE308BD63D36FB6025EEEAFBEDB97C67C1* ___m_ParticleSystem_0;
};
// Native definition for P/Invoke marshalling of UnityEngine.ParticleSystem/SubEmittersModule
struct SubEmittersModule_t94F5AD231EAFB50A16E697186A630B07BF8B949B_marshaled_pinvoke
{
	ParticleSystem_tB19986EE308BD63D36FB6025EEEAFBEDB97C67C1* ___m_ParticleSystem_0;
};
// Native definition for COM marshalling of UnityEngine.ParticleSystem/SubEmittersModule
struct SubEmittersModule_t94F5AD231EAFB50A16E697186A630B07BF8B949B_marshaled_com
{
	ParticleSystem_tB19986EE308BD63D36FB6025EEEAFBEDB97C67C1* ___m_ParticleSystem_0;
};

// UnityEngine.Networking.DownloadHandler
struct DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB  : public RuntimeObject
{
	// System.IntPtr UnityEngine.Networking.DownloadHandler::m_Ptr
	intptr_t ___m_Ptr_0;
};
// Native definition for P/Invoke marshalling of UnityEngine.Networking.DownloadHandler
struct DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
};
// Native definition for COM marshalling of UnityEngine.Networking.DownloadHandler
struct DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_com
{
	intptr_t ___m_Ptr_0;
};

// UnityEngine.IntegratedSubsystem
struct IntegratedSubsystem_t990160A89854D87C0836DC589B720231C02D4CE3  : public RuntimeObject
{
	// System.IntPtr UnityEngine.IntegratedSubsystem::m_Ptr
	intptr_t ___m_Ptr_0;
	// UnityEngine.ISubsystemDescriptor UnityEngine.IntegratedSubsystem::m_SubsystemDescriptor
	RuntimeObject* ___m_SubsystemDescriptor_1;
};
// Native definition for P/Invoke marshalling of UnityEngine.IntegratedSubsystem
struct IntegratedSubsystem_t990160A89854D87C0836DC589B720231C02D4CE3_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
	RuntimeObject* ___m_SubsystemDescriptor_1;
};
// Native definition for COM marshalling of UnityEngine.IntegratedSubsystem
struct IntegratedSubsystem_t990160A89854D87C0836DC589B720231C02D4CE3_marshaled_com
{
	intptr_t ___m_Ptr_0;
	RuntimeObject* ___m_SubsystemDescriptor_1;
};

// UnityEngine.IntegratedSubsystemDescriptor
struct IntegratedSubsystemDescriptor_t9232963B842E01748A8E032928DC8E35DF00C10D  : public RuntimeObject
{
	// System.IntPtr UnityEngine.IntegratedSubsystemDescriptor::m_Ptr
	intptr_t ___m_Ptr_0;
};
// Native definition for P/Invoke marshalling of UnityEngine.IntegratedSubsystemDescriptor
struct IntegratedSubsystemDescriptor_t9232963B842E01748A8E032928DC8E35DF00C10D_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
};
// Native definition for COM marshalling of UnityEngine.IntegratedSubsystemDescriptor
struct IntegratedSubsystemDescriptor_t9232963B842E01748A8E032928DC8E35DF00C10D_marshaled_com
{
	intptr_t ___m_Ptr_0;
};

// UnityEngine.Object
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C  : public RuntimeObject
{
	// System.IntPtr UnityEngine.Object::m_CachedPtr
	intptr_t ___m_CachedPtr_0;
};

struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_StaticFields
{
	// System.Int32 UnityEngine.Object::OffsetOfInstanceIDInCPlusPlusObject
	int32_t ___OffsetOfInstanceIDInCPlusPlusObject_1;
};
// Native definition for P/Invoke marshalling of UnityEngine.Object
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke
{
	intptr_t ___m_CachedPtr_0;
};
// Native definition for COM marshalling of UnityEngine.Object
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com
{
	intptr_t ___m_CachedPtr_0;
};

// Boxophobic.StyledGUI.StyledBanner
struct StyledBanner_t9563605035FD8A4DC8E5035F1A996B861CC1873C  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.Single Boxophobic.StyledGUI.StyledBanner::colorR
	float ___colorR_0;
	// System.Single Boxophobic.StyledGUI.StyledBanner::colorG
	float ___colorG_1;
	// System.Single Boxophobic.StyledGUI.StyledBanner::colorB
	float ___colorB_2;
	// System.String Boxophobic.StyledGUI.StyledBanner::title
	String_t* ___title_3;
	// System.String Boxophobic.StyledGUI.StyledBanner::helpURL
	String_t* ___helpURL_4;
};

// Boxophobic.StyledGUI.StyledButton
struct StyledButton_tA40AC186DFB9B4850C2A844A31E8E91E85007516  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.String Boxophobic.StyledGUI.StyledButton::Text
	String_t* ___Text_0;
	// System.Single Boxophobic.StyledGUI.StyledButton::Top
	float ___Top_1;
	// System.Single Boxophobic.StyledGUI.StyledButton::Down
	float ___Down_2;
};

// Boxophobic.StyledGUI.StyledCategory
struct StyledCategory_t56EA34A1E7A8452D3DAF2B5066286B571539C9BE  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.String Boxophobic.StyledGUI.StyledCategory::category
	String_t* ___category_0;
	// System.Single Boxophobic.StyledGUI.StyledCategory::top
	float ___top_1;
	// System.Single Boxophobic.StyledGUI.StyledCategory::down
	float ___down_2;
	// System.Boolean Boxophobic.StyledGUI.StyledCategory::colapsable
	bool ___colapsable_3;
};

// Boxophobic.StyledGUI.StyledEnum
struct StyledEnum_tF11D18EC6AA2130E13FDFAE04EDCF989C7444F36  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.String Boxophobic.StyledGUI.StyledEnum::display
	String_t* ___display_0;
	// System.String Boxophobic.StyledGUI.StyledEnum::file
	String_t* ___file_1;
	// System.String Boxophobic.StyledGUI.StyledEnum::options
	String_t* ___options_2;
	// System.Int32 Boxophobic.StyledGUI.StyledEnum::top
	int32_t ___top_3;
	// System.Int32 Boxophobic.StyledGUI.StyledEnum::down
	int32_t ___down_4;
};

// Boxophobic.StyledGUI.StyledIndent
struct StyledIndent_tBD1203836B149BBDACAA229BFBE875874482B35C  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.Int32 Boxophobic.StyledGUI.StyledIndent::indent
	int32_t ___indent_0;
};

// Boxophobic.StyledGUI.StyledLayers
struct StyledLayers_tAFED366047AA4A071471268AE19FFDA1D0CD250E  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.String Boxophobic.StyledGUI.StyledLayers::display
	String_t* ___display_0;
};

// Boxophobic.StyledGUI.StyledMask
struct StyledMask_t360C2709D699BB12A2A7A6A7193585C08433F6FE  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.String Boxophobic.StyledGUI.StyledMask::display
	String_t* ___display_0;
	// System.String Boxophobic.StyledGUI.StyledMask::file
	String_t* ___file_1;
	// System.String Boxophobic.StyledGUI.StyledMask::options
	String_t* ___options_2;
	// System.Int32 Boxophobic.StyledGUI.StyledMask::top
	int32_t ___top_3;
	// System.Int32 Boxophobic.StyledGUI.StyledMask::down
	int32_t ___down_4;
};

// Boxophobic.StyledGUI.StyledMessage
struct StyledMessage_t9125BC8B5131542BF991B29F46D2AFF0B0CD991C  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.String Boxophobic.StyledGUI.StyledMessage::Type
	String_t* ___Type_0;
	// System.String Boxophobic.StyledGUI.StyledMessage::Message
	String_t* ___Message_1;
	// System.Single Boxophobic.StyledGUI.StyledMessage::Top
	float ___Top_2;
	// System.Single Boxophobic.StyledGUI.StyledMessage::Down
	float ___Down_3;
};

// Boxophobic.StyledGUI.StyledRangeOptions
struct StyledRangeOptions_t6168F2B8FD5CCCB82C26242146A6CFE4B4C0D6AA  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.String Boxophobic.StyledGUI.StyledRangeOptions::display
	String_t* ___display_0;
	// System.Single Boxophobic.StyledGUI.StyledRangeOptions::min
	float ___min_1;
	// System.Single Boxophobic.StyledGUI.StyledRangeOptions::max
	float ___max_2;
	// System.String[] Boxophobic.StyledGUI.StyledRangeOptions::options
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___options_3;
};

// Boxophobic.StyledGUI.StyledSpace
struct StyledSpace_t59FD27079C76ABD7E62742DD3FC6B4D1A44F3C7C  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.Int32 Boxophobic.StyledGUI.StyledSpace::space
	int32_t ___space_0;
};

// Boxophobic.StyledGUI.StyledText
struct StyledText_t6AF253489FCD62B6143C76036C6385023FF1FE56  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.String Boxophobic.StyledGUI.StyledText::text
	String_t* ___text_0;
	// UnityEngine.TextAnchor Boxophobic.StyledGUI.StyledText::alignment
	int32_t ___alignment_1;
	// System.Single Boxophobic.StyledGUI.StyledText::top
	float ___top_2;
	// System.Single Boxophobic.StyledGUI.StyledText::down
	float ___down_3;
};

// Boxophobic.StyledGUI.StyledTexturePreview
struct StyledTexturePreview_tB32E2E0081B25926B06D4DEE527D9B189F8575AF  : public PropertyAttribute_t5E0CB5A6CDA6E24CBD4FF26DE3B0C29D8BB54BF0
{
	// System.String Boxophobic.StyledGUI.StyledTexturePreview::displayName
	String_t* ___displayName_0;
};

// UnityEngine.ParticleSystem/Particle
struct Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D 
{
	// UnityEngine.Vector3 UnityEngine.ParticleSystem/Particle::m_Position
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___m_Position_0;
	// UnityEngine.Vector3 UnityEngine.ParticleSystem/Particle::m_Velocity
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___m_Velocity_1;
	// UnityEngine.Vector3 UnityEngine.ParticleSystem/Particle::m_AnimatedVelocity
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___m_AnimatedVelocity_2;
	// UnityEngine.Vector3 UnityEngine.ParticleSystem/Particle::m_InitialVelocity
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___m_InitialVelocity_3;
	// UnityEngine.Vector3 UnityEngine.ParticleSystem/Particle::m_AxisOfRotation
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___m_AxisOfRotation_4;
	// UnityEngine.Vector3 UnityEngine.ParticleSystem/Particle::m_Rotation
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___m_Rotation_5;
	// UnityEngine.Vector3 UnityEngine.ParticleSystem/Particle::m_AngularVelocity
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___m_AngularVelocity_6;
	// UnityEngine.Vector3 UnityEngine.ParticleSystem/Particle::m_StartSize
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___m_StartSize_7;
	// UnityEngine.Color32 UnityEngine.ParticleSystem/Particle::m_StartColor
	Color32_t73C5004937BF5BB8AD55323D51AAA40A898EF48B ___m_StartColor_8;
	// System.UInt32 UnityEngine.ParticleSystem/Particle::m_RandomSeed
	uint32_t ___m_RandomSeed_9;
	// System.UInt32 UnityEngine.ParticleSystem/Particle::m_ParentRandomSeed
	uint32_t ___m_ParentRandomSeed_10;
	// System.Single UnityEngine.ParticleSystem/Particle::m_Lifetime
	float ___m_Lifetime_11;
	// System.Single UnityEngine.ParticleSystem/Particle::m_StartLifetime
	float ___m_StartLifetime_12;
	// System.Int32 UnityEngine.ParticleSystem/Particle::m_MeshIndex
	int32_t ___m_MeshIndex_13;
	// System.Single UnityEngine.ParticleSystem/Particle::m_EmitAccumulator0
	float ___m_EmitAccumulator0_14;
	// System.Single UnityEngine.ParticleSystem/Particle::m_EmitAccumulator1
	float ___m_EmitAccumulator1_15;
	// System.UInt32 UnityEngine.ParticleSystem/Particle::m_Flags
	uint32_t ___m_Flags_16;
};

// UnityEngine.Component
struct Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3  : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C
{
};

// UnityEngine.Networking.DownloadHandlerTexture
struct DownloadHandlerTexture_t45E2D719000AA1594E648810F0B57A77FA7C568C  : public DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB
{
	// Unity.Collections.NativeArray`1<System.Byte> UnityEngine.Networking.DownloadHandlerTexture::m_NativeData
	NativeArray_1_t81F55263465517B73C455D3400CF67B4BADD85CF ___m_NativeData_1;
	// UnityEngine.Texture2D UnityEngine.Networking.DownloadHandlerTexture::mTexture
	Texture2D_tE6505BC111DD8A424A9DBE8E05D7D09E11FFFCF4* ___mTexture_2;
	// System.Boolean UnityEngine.Networking.DownloadHandlerTexture::mHasTexture
	bool ___mHasTexture_3;
	// System.Boolean UnityEngine.Networking.DownloadHandlerTexture::mNonReadable
	bool ___mNonReadable_4;
};
// Native definition for P/Invoke marshalling of UnityEngine.Networking.DownloadHandlerTexture
struct DownloadHandlerTexture_t45E2D719000AA1594E648810F0B57A77FA7C568C_marshaled_pinvoke : public DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_pinvoke
{
	NativeArray_1_t81F55263465517B73C455D3400CF67B4BADD85CF ___m_NativeData_1;
	Texture2D_tE6505BC111DD8A424A9DBE8E05D7D09E11FFFCF4* ___mTexture_2;
	int32_t ___mHasTexture_3;
	int32_t ___mNonReadable_4;
};
// Native definition for COM marshalling of UnityEngine.Networking.DownloadHandlerTexture
struct DownloadHandlerTexture_t45E2D719000AA1594E648810F0B57A77FA7C568C_marshaled_com : public DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_com
{
	NativeArray_1_t81F55263465517B73C455D3400CF67B4BADD85CF ___m_NativeData_1;
	Texture2D_tE6505BC111DD8A424A9DBE8E05D7D09E11FFFCF4* ___mTexture_2;
	int32_t ___mHasTexture_3;
	int32_t ___mNonReadable_4;
};

// UnityEngine.ScriptableObject
struct ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A  : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C
{
};
// Native definition for P/Invoke marshalling of UnityEngine.ScriptableObject
struct ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A_marshaled_pinvoke : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke
{
};
// Native definition for COM marshalling of UnityEngine.ScriptableObject
struct ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A_marshaled_com : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com
{
};

// UnityEngine.ParticleSystem/EmitParams
struct EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0 
{
	// UnityEngine.ParticleSystem/Particle UnityEngine.ParticleSystem/EmitParams::m_Particle
	Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D ___m_Particle_0;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_PositionSet
	bool ___m_PositionSet_1;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_VelocitySet
	bool ___m_VelocitySet_2;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_AxisOfRotationSet
	bool ___m_AxisOfRotationSet_3;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_RotationSet
	bool ___m_RotationSet_4;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_AngularVelocitySet
	bool ___m_AngularVelocitySet_5;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_StartSizeSet
	bool ___m_StartSizeSet_6;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_StartColorSet
	bool ___m_StartColorSet_7;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_RandomSeedSet
	bool ___m_RandomSeedSet_8;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_StartLifetimeSet
	bool ___m_StartLifetimeSet_9;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_MeshIndexSet
	bool ___m_MeshIndexSet_10;
	// System.Boolean UnityEngine.ParticleSystem/EmitParams::m_ApplyShapeToPosition
	bool ___m_ApplyShapeToPosition_11;
};
// Native definition for P/Invoke marshalling of UnityEngine.ParticleSystem/EmitParams
struct EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0_marshaled_pinvoke
{
	Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D ___m_Particle_0;
	int32_t ___m_PositionSet_1;
	int32_t ___m_VelocitySet_2;
	int32_t ___m_AxisOfRotationSet_3;
	int32_t ___m_RotationSet_4;
	int32_t ___m_AngularVelocitySet_5;
	int32_t ___m_StartSizeSet_6;
	int32_t ___m_StartColorSet_7;
	int32_t ___m_RandomSeedSet_8;
	int32_t ___m_StartLifetimeSet_9;
	int32_t ___m_MeshIndexSet_10;
	int32_t ___m_ApplyShapeToPosition_11;
};
// Native definition for COM marshalling of UnityEngine.ParticleSystem/EmitParams
struct EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0_marshaled_com
{
	Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D ___m_Particle_0;
	int32_t ___m_PositionSet_1;
	int32_t ___m_VelocitySet_2;
	int32_t ___m_AxisOfRotationSet_3;
	int32_t ___m_RotationSet_4;
	int32_t ___m_AngularVelocitySet_5;
	int32_t ___m_StartSizeSet_6;
	int32_t ___m_StartColorSet_7;
	int32_t ___m_RandomSeedSet_8;
	int32_t ___m_StartLifetimeSet_9;
	int32_t ___m_MeshIndexSet_10;
	int32_t ___m_ApplyShapeToPosition_11;
};

// UnityEngine.Behaviour
struct Behaviour_t01970CFBBA658497AE30F311C447DB0440BAB7FA  : public Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3
{
};

// Boxophobic.Utils.SettingsData
struct SettingsData_t4DBBEEB21A569B3218B58C37A1E16CC0499139E8  : public ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A
{
	// System.String Boxophobic.Utils.SettingsData::data
	String_t* ___data_4;
};

// UnityEngine.Playables.PlayableDirector
struct PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475  : public Behaviour_t01970CFBBA658497AE30F311C447DB0440BAB7FA
{
	// System.Action`1<UnityEngine.Playables.PlayableDirector> UnityEngine.Playables.PlayableDirector::played
	Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* ___played_4;
	// System.Action`1<UnityEngine.Playables.PlayableDirector> UnityEngine.Playables.PlayableDirector::paused
	Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* ___paused_5;
	// System.Action`1<UnityEngine.Playables.PlayableDirector> UnityEngine.Playables.PlayableDirector::stopped
	Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* ___stopped_6;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif



#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6002[5] = 
{
	static_cast<int32_t>(sizeof(RuntimeObject)),0,0,0,0,};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6003[2] = 
{
	static_cast<int32_t>(offsetof(StaticAccessorAttribute_tDE194716AED7A414D473DC570B2E0035A5CE130A, ___U3CNameU3Ek__BackingField_0)),static_cast<int32_t>(offsetof(StaticAccessorAttribute_tDE194716AED7A414D473DC570B2E0035A5CE130A, ___U3CTypeU3Ek__BackingField_1)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6004[1] = 
{
	static_cast<int32_t>(offsetof(NativeThrowsAttribute_t211CE8D047A8D45676C9ED399D5AA3B4A2C3E625, ___U3CThrowsExceptionU3Ek__BackingField_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6005[1] = 
{
	static_cast<int32_t>(offsetof(IgnoreAttribute_tAB3F6C4808BA16CD585D60A6353B3E0599DFCE4D, ___U3CDoesNotContributeToSizeU3Ek__BackingField_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6007[1] = 
{
	static_cast<int32_t>(offsetof(UsedByNativeCodeAttribute_t3FE9A7CDCC6A3A4122D8BF44F1D0A37BB38894C1, ___U3CNameU3Ek__BackingField_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6008[3] = 
{
	static_cast<int32_t>(offsetof(RequiredByNativeCodeAttribute_t86B11F2BA12BB463CE3258E64E16B43484014FCA, ___U3CNameU3Ek__BackingField_0)),static_cast<int32_t>(offsetof(RequiredByNativeCodeAttribute_t86B11F2BA12BB463CE3258E64E16B43484014FCA, ___U3COptionalU3Ek__BackingField_1)),static_cast<int32_t>(offsetof(RequiredByNativeCodeAttribute_t86B11F2BA12BB463CE3258E64E16B43484014FCA, ___U3CGenerateProxyU3Ek__BackingField_2)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6010[1] = 
{
	static_cast<int32_t>(offsetof(MainModule_tC7ECD8330C14B0808478A748048988A6085CE2A9, ___m_ParticleSystem_0)) + static_cast<int32_t>(sizeof(RuntimeObject)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6011[1] = 
{
	static_cast<int32_t>(offsetof(SubEmittersModule_t94F5AD231EAFB50A16E697186A630B07BF8B949B, ___m_ParticleSystem_0)) + static_cast<int32_t>(sizeof(RuntimeObject)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6012[17] = 
{
	static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_Position_0)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_Velocity_1)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_AnimatedVelocity_2)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_InitialVelocity_3)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_AxisOfRotation_4)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_Rotation_5)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_AngularVelocity_6)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_StartSize_7)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_StartColor_8)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_RandomSeed_9)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_ParentRandomSeed_10)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_Lifetime_11)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_StartLifetime_12)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_MeshIndex_13)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_EmitAccumulator0_14)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_EmitAccumulator1_15)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(Particle_tF16C89682A98AB276CCBE4DA0A6E82F98500F79D, ___m_Flags_16)) + static_cast<int32_t>(sizeof(RuntimeObject)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6013[12] = 
{
	static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_Particle_0)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_PositionSet_1)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_VelocitySet_2)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_AxisOfRotationSet_3)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_RotationSet_4)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_AngularVelocitySet_5)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_StartSizeSet_6)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_StartColorSet_7)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_RandomSeedSet_8)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_StartLifetimeSet_9)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_MeshIndexSet_10)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(EmitParams_tE76279CE754C7B0A4ECDA7E294587AACB039FBA0, ___m_ApplyShapeToPosition_11)) + static_cast<int32_t>(sizeof(RuntimeObject)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6015[3] = 
{
	static_cast<int32_t>(sizeof(RuntimeObject)),0,0,};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6018[2] = 
{
	static_cast<int32_t>(offsetof(IntegratedSubsystem_t990160A89854D87C0836DC589B720231C02D4CE3, ___m_Ptr_0)),static_cast<int32_t>(offsetof(IntegratedSubsystem_t990160A89854D87C0836DC589B720231C02D4CE3, ___m_SubsystemDescriptor_1)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6020[1] = 
{
	static_cast<int32_t>(offsetof(IntegratedSubsystemDescriptor_t9232963B842E01748A8E032928DC8E35DF00C10D, ___m_Ptr_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6025[1] = 
{
	static_cast<int32_t>(offsetof(SubsystemDescriptor_tF417D2751C69A8B0DD86162EBCE55F84D3493A71, ___U3CidU3Ek__BackingField_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6027[7] = 
{
	static_cast<int32_t>(offsetof(SubsystemManager_t9A7261E4D0B53B996F04B8707D8E1C33AB65E824_StaticFields, ___beforeReloadSubsystems_0)),static_cast<int32_t>(offsetof(SubsystemManager_t9A7261E4D0B53B996F04B8707D8E1C33AB65E824_StaticFields, ___afterReloadSubsystems_1)),static_cast<int32_t>(offsetof(SubsystemManager_t9A7261E4D0B53B996F04B8707D8E1C33AB65E824_StaticFields, ___s_IntegratedSubsystems_2)),static_cast<int32_t>(offsetof(SubsystemManager_t9A7261E4D0B53B996F04B8707D8E1C33AB65E824_StaticFields, ___s_StandaloneSubsystems_3)),static_cast<int32_t>(offsetof(SubsystemManager_t9A7261E4D0B53B996F04B8707D8E1C33AB65E824_StaticFields, ___s_DeprecatedSubsystems_4)),static_cast<int32_t>(offsetof(SubsystemManager_t9A7261E4D0B53B996F04B8707D8E1C33AB65E824_StaticFields, ___reloadSubsytemsStarted_5)),static_cast<int32_t>(offsetof(SubsystemManager_t9A7261E4D0B53B996F04B8707D8E1C33AB65E824_StaticFields, ___reloadSubsytemsCompleted_6)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6028[3] = 
{
	static_cast<int32_t>(offsetof(SubsystemDescriptorStore_tEF3761B84B8C25EA4B93F94A487551820B268250_StaticFields, ___s_IntegratedDescriptors_0)),static_cast<int32_t>(offsetof(SubsystemDescriptorStore_tEF3761B84B8C25EA4B93F94A487551820B268250_StaticFields, ___s_StandaloneDescriptors_1)),static_cast<int32_t>(offsetof(SubsystemDescriptorStore_tEF3761B84B8C25EA4B93F94A487551820B268250_StaticFields, ___s_DeprecatedDescriptors_2)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6029[1] = 
{
	static_cast<int32_t>(offsetof(SubsystemDescriptorWithProvider_t2A61A2C951A4A179E898CF207726BF6B5AF474D5, ___U3CidU3Ek__BackingField_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6032[3] = 
{
	static_cast<int32_t>(offsetof(PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475, ___played_4)),static_cast<int32_t>(offsetof(PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475, ___paused_5)),static_cast<int32_t>(offsetof(PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475, ___stopped_6)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6035[4] = 
{
	static_cast<int32_t>(offsetof(DownloadHandlerTexture_t45E2D719000AA1594E648810F0B57A77FA7C568C, ___m_NativeData_1)),static_cast<int32_t>(offsetof(DownloadHandlerTexture_t45E2D719000AA1594E648810F0B57A77FA7C568C, ___mTexture_2)),static_cast<int32_t>(offsetof(DownloadHandlerTexture_t45E2D719000AA1594E648810F0B57A77FA7C568C, ___mHasTexture_3)),static_cast<int32_t>(offsetof(DownloadHandlerTexture_t45E2D719000AA1594E648810F0B57A77FA7C568C, ___mNonReadable_4)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6038[4] = 
{
	static_cast<int32_t>(offsetof(NativeInputEventBuffer_t4EE5873AD7998E0E83C9F8585C338AB14C9101FD, ___eventBuffer_0)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(NativeInputEventBuffer_t4EE5873AD7998E0E83C9F8585C338AB14C9101FD, ___eventCount_1)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(NativeInputEventBuffer_t4EE5873AD7998E0E83C9F8585C338AB14C9101FD, ___sizeInBytes_2)) + static_cast<int32_t>(sizeof(RuntimeObject)),static_cast<int32_t>(offsetof(NativeInputEventBuffer_t4EE5873AD7998E0E83C9F8585C338AB14C9101FD, ___capacityInBytes_3)) + static_cast<int32_t>(sizeof(RuntimeObject)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6039[6] = 
{
	static_cast<int32_t>(sizeof(RuntimeObject)),0,0,0,0,0,};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6040[4] = 
{
	static_cast<int32_t>(offsetof(NativeInputSystem_tCFE5554EBC0D3EE1DAD80FC55CE0DE38A3DDC5EE_StaticFields, ___onUpdate_0)),static_cast<int32_t>(offsetof(NativeInputSystem_tCFE5554EBC0D3EE1DAD80FC55CE0DE38A3DDC5EE_StaticFields, ___onBeforeUpdate_1)),static_cast<int32_t>(offsetof(NativeInputSystem_tCFE5554EBC0D3EE1DAD80FC55CE0DE38A3DDC5EE_StaticFields, ___onShouldRunUpdate_2)),static_cast<int32_t>(offsetof(NativeInputSystem_tCFE5554EBC0D3EE1DAD80FC55CE0DE38A3DDC5EE_StaticFields, ___s_OnDeviceDiscoveredCallback_3)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6042[5] = 
{
	static_cast<int32_t>(sizeof(RuntimeObject)),0,0,0,0,};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6044[1] = 
{
	static_cast<int32_t>(offsetof(XRDevice_tD076A68EFE413B3EEEEA362BE0364A488B58F194_StaticFields, ___deviceLoaded_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6059[5] = 
{
	static_cast<int32_t>(offsetof(StyledBanner_t9563605035FD8A4DC8E5035F1A996B861CC1873C, ___colorR_0)),static_cast<int32_t>(offsetof(StyledBanner_t9563605035FD8A4DC8E5035F1A996B861CC1873C, ___colorG_1)),static_cast<int32_t>(offsetof(StyledBanner_t9563605035FD8A4DC8E5035F1A996B861CC1873C, ___colorB_2)),static_cast<int32_t>(offsetof(StyledBanner_t9563605035FD8A4DC8E5035F1A996B861CC1873C, ___title_3)),static_cast<int32_t>(offsetof(StyledBanner_t9563605035FD8A4DC8E5035F1A996B861CC1873C, ___helpURL_4)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6060[3] = 
{
	static_cast<int32_t>(offsetof(StyledButton_tA40AC186DFB9B4850C2A844A31E8E91E85007516, ___Text_0)),static_cast<int32_t>(offsetof(StyledButton_tA40AC186DFB9B4850C2A844A31E8E91E85007516, ___Top_1)),static_cast<int32_t>(offsetof(StyledButton_tA40AC186DFB9B4850C2A844A31E8E91E85007516, ___Down_2)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6061[4] = 
{
	static_cast<int32_t>(offsetof(StyledCategory_t56EA34A1E7A8452D3DAF2B5066286B571539C9BE, ___category_0)),static_cast<int32_t>(offsetof(StyledCategory_t56EA34A1E7A8452D3DAF2B5066286B571539C9BE, ___top_1)),static_cast<int32_t>(offsetof(StyledCategory_t56EA34A1E7A8452D3DAF2B5066286B571539C9BE, ___down_2)),static_cast<int32_t>(offsetof(StyledCategory_t56EA34A1E7A8452D3DAF2B5066286B571539C9BE, ___colapsable_3)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6062[5] = 
{
	static_cast<int32_t>(offsetof(StyledEnum_tF11D18EC6AA2130E13FDFAE04EDCF989C7444F36, ___display_0)),static_cast<int32_t>(offsetof(StyledEnum_tF11D18EC6AA2130E13FDFAE04EDCF989C7444F36, ___file_1)),static_cast<int32_t>(offsetof(StyledEnum_tF11D18EC6AA2130E13FDFAE04EDCF989C7444F36, ___options_2)),static_cast<int32_t>(offsetof(StyledEnum_tF11D18EC6AA2130E13FDFAE04EDCF989C7444F36, ___top_3)),static_cast<int32_t>(offsetof(StyledEnum_tF11D18EC6AA2130E13FDFAE04EDCF989C7444F36, ___down_4)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6063[1] = 
{
	static_cast<int32_t>(offsetof(StyledIndent_tBD1203836B149BBDACAA229BFBE875874482B35C, ___indent_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6065[1] = 
{
	static_cast<int32_t>(offsetof(StyledLayers_tAFED366047AA4A071471268AE19FFDA1D0CD250E, ___display_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6066[5] = 
{
	static_cast<int32_t>(offsetof(StyledMask_t360C2709D699BB12A2A7A6A7193585C08433F6FE, ___display_0)),static_cast<int32_t>(offsetof(StyledMask_t360C2709D699BB12A2A7A6A7193585C08433F6FE, ___file_1)),static_cast<int32_t>(offsetof(StyledMask_t360C2709D699BB12A2A7A6A7193585C08433F6FE, ___options_2)),static_cast<int32_t>(offsetof(StyledMask_t360C2709D699BB12A2A7A6A7193585C08433F6FE, ___top_3)),static_cast<int32_t>(offsetof(StyledMask_t360C2709D699BB12A2A7A6A7193585C08433F6FE, ___down_4)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6067[4] = 
{
	static_cast<int32_t>(offsetof(StyledMessage_t9125BC8B5131542BF991B29F46D2AFF0B0CD991C, ___Type_0)),static_cast<int32_t>(offsetof(StyledMessage_t9125BC8B5131542BF991B29F46D2AFF0B0CD991C, ___Message_1)),static_cast<int32_t>(offsetof(StyledMessage_t9125BC8B5131542BF991B29F46D2AFF0B0CD991C, ___Top_2)),static_cast<int32_t>(offsetof(StyledMessage_t9125BC8B5131542BF991B29F46D2AFF0B0CD991C, ___Down_3)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6068[4] = 
{
	static_cast<int32_t>(offsetof(StyledRangeOptions_t6168F2B8FD5CCCB82C26242146A6CFE4B4C0D6AA, ___display_0)),static_cast<int32_t>(offsetof(StyledRangeOptions_t6168F2B8FD5CCCB82C26242146A6CFE4B4C0D6AA, ___min_1)),static_cast<int32_t>(offsetof(StyledRangeOptions_t6168F2B8FD5CCCB82C26242146A6CFE4B4C0D6AA, ___max_2)),static_cast<int32_t>(offsetof(StyledRangeOptions_t6168F2B8FD5CCCB82C26242146A6CFE4B4C0D6AA, ___options_3)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6069[1] = 
{
	static_cast<int32_t>(offsetof(StyledSpace_t59FD27079C76ABD7E62742DD3FC6B4D1A44F3C7C, ___space_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6070[4] = 
{
	static_cast<int32_t>(offsetof(StyledText_t6AF253489FCD62B6143C76036C6385023FF1FE56, ___text_0)),static_cast<int32_t>(offsetof(StyledText_t6AF253489FCD62B6143C76036C6385023FF1FE56, ___alignment_1)),static_cast<int32_t>(offsetof(StyledText_t6AF253489FCD62B6143C76036C6385023FF1FE56, ___top_2)),static_cast<int32_t>(offsetof(StyledText_t6AF253489FCD62B6143C76036C6385023FF1FE56, ___down_3)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6071[1] = 
{
	static_cast<int32_t>(offsetof(StyledTexturePreview_tB32E2E0081B25926B06D4DEE527D9B189F8575AF, ___displayName_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6072[1] = 
{
	static_cast<int32_t>(offsetof(SettingsData_t4DBBEEB21A569B3218B58C37A1E16CC0499139E8, ___data_4)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable6076[4] = 
{
	static_cast<int32_t>(sizeof(RuntimeObject)),0,0,0,};
