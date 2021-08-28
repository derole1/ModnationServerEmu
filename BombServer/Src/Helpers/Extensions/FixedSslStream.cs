using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Security;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Reflection;

namespace BombServerEmu_MNR.Src.Helpers.Extensions
{
    public class FixedSslStream : SslStream
    {
        public FixedSslStream(Stream innerStream)
            : base(innerStream)
        {
        }
        public FixedSslStream(Stream innerStream, bool leaveInnerStreamOpen)
            : base(innerStream, leaveInnerStreamOpen)
        {
        }
        public FixedSslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback)
            : base(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback)
        {
        }
        public FixedSslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback)
            : base(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback)
        {
        }
        public FixedSslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback, EncryptionPolicy encryptionPolicy)
            : base(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback, encryptionPolicy)
        {
        }
        public override void Close()
        {
            try
            {
                SslDirectCall.CloseNotify(this);
            }
            finally
            {
                base.Close();
            }
        }
    }

    public unsafe static class NativeApi
    {
        internal enum BufferType
        {
            Empty,
            Data,
            Token,
            Parameters,
            Missing,
            Extra,
            Trailer,
            Header,
            Padding = 9,
            Stream,
            ChannelBindings = 14,
            TargetHost = 16,
            ReadOnlyFlag = -2147483648,
            ReadOnlyWithChecksum = 268435456
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct SSPIHandle
        {
            public IntPtr HandleHi;
            public IntPtr HandleLo;
            public bool IsZero
            {
                get
                {
                    return this.HandleHi == IntPtr.Zero && this.HandleLo == IntPtr.Zero;
                }
            }
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            internal void SetToInvalid()
            {
                this.HandleHi = IntPtr.Zero;
                this.HandleLo = IntPtr.Zero;
            }
            public override string ToString()
            {
                return this.HandleHi.ToString("x") + ":" + this.HandleLo.ToString("x");
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class SecurityBufferDescriptor
        {
            public readonly int Version;
            public readonly int Count;
            public unsafe void* UnmanagedPointer;
            public SecurityBufferDescriptor(int count)
            {
                this.Version = 0;
                this.Count = count;
                this.UnmanagedPointer = null;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SecurityBufferStruct
        {
            public int count;
            public BufferType type;
            public IntPtr token;
            public static readonly int Size = sizeof(SecurityBufferStruct);
        }

        internal enum SecurityStatus
        {
            OK,
            ContinueNeeded = 590610,
            CompleteNeeded,
            CompAndContinue,
            ContextExpired = 590615,
            CredentialsNeeded = 590624,
            Renegotiate,
            OutOfMemory = -2146893056,
            InvalidHandle,
            Unsupported,
            TargetUnknown,
            InternalError,
            PackageNotFound,
            NotOwner,
            CannotInstall,
            InvalidToken,
            CannotPack,
            QopNotSupported,
            NoImpersonation,
            LogonDenied,
            UnknownCredentials,
            NoCredentials,
            MessageAltered,
            OutOfSequence,
            NoAuthenticatingAuthority,
            IncompleteMessage = -2146893032,
            IncompleteCredentials = -2146893024,
            BufferNotEnough,
            WrongPrincipal,
            TimeSkew = -2146893020,
            UntrustedRoot,
            IllegalMessage,
            CertUnknown,
            CertExpired,
            AlgorithmMismatch = -2146893007,
            SecurityQosFailed,
            SmartcardLogonRequired = -2146892994,
            UnsupportedPreauth = -2146892989,
            BadBinding = -2146892986
        }
        [Flags]
        internal enum ContextFlags
        {
            Zero = 0,
            Delegate = 1,
            MutualAuth = 2,
            ReplayDetect = 4,
            SequenceDetect = 8,
            Confidentiality = 16,
            UseSessionKey = 32,
            AllocateMemory = 256,
            Connection = 2048,
            InitExtendedError = 16384,
            AcceptExtendedError = 32768,
            InitStream = 32768,
            AcceptStream = 65536,
            InitIntegrity = 65536,
            AcceptIntegrity = 131072,
            InitManualCredValidation = 524288,
            InitUseSuppliedCreds = 128,
            InitIdentify = 131072,
            AcceptIdentify = 524288,
            ProxyBindings = 67108864,
            AllowMissingBindings = 268435456,
            UnverifiedTargetName = 536870912
        }
        internal enum Endianness
        {
            Network,
            Native = 16
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport("secur32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern int ApplyControlToken(ref SSPIHandle contextHandle, [In] [Out] SecurityBufferDescriptor outputBuffer);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport("secur32.dll", ExactSpelling = true, SetLastError = true)]
        internal unsafe static extern int AcceptSecurityContext(ref SSPIHandle credentialHandle, ref SSPIHandle contextHandle, [In] SecurityBufferDescriptor inputBuffer, [In] ContextFlags inFlags, [In] Endianness endianness, ref SSPIHandle outContextPtr, [In] [Out] SecurityBufferDescriptor outputBuffer, [In] [Out] ref ContextFlags attributes, out long timeStamp);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport("secur32.dll", ExactSpelling = true, SetLastError = true)]
        internal unsafe static extern int InitializeSecurityContextW(ref SSPIHandle credentialHandle, ref SSPIHandle contextHandle, [In] byte* targetName, [In] ContextFlags inFlags, [In] int reservedI, [In] Endianness endianness, [In] SecurityBufferDescriptor inputBuffer, [In] int reservedII, ref SSPIHandle outContextPtr, [In] [Out] SecurityBufferDescriptor outputBuffer, [In] [Out] ref ContextFlags attributes, out long timeStamp);
    }

    public unsafe static class SslDirectCall
    {
        public static void CloseNotify(SslStream sslStream)
        {
            if (sslStream.IsAuthenticated)
            {
                bool isServer = sslStream.IsServer;

                byte[] result;
                int resultSz;
                var asmbSystem = typeof(System.Net.Authorization).Assembly;

                int SCHANNEL_SHUTDOWN = 1;
                var workArray = BitConverter.GetBytes(SCHANNEL_SHUTDOWN);

                var sslstate = ReflectUtil.GetField(sslStream, "_SslState");
                var context = ReflectUtil.GetProperty(sslstate, "Context");

                var securityContext = ReflectUtil.GetField(context, "m_SecurityContext");
                var securityContextHandleOriginal = ReflectUtil.GetField(securityContext, "_handle");
                NativeApi.SSPIHandle securityContextHandle = default(NativeApi.SSPIHandle);
                securityContextHandle.HandleHi = (IntPtr)ReflectUtil.GetField(securityContextHandleOriginal, "HandleHi");
                securityContextHandle.HandleLo = (IntPtr)ReflectUtil.GetField(securityContextHandleOriginal, "HandleLo");

                var credentialsHandle = ReflectUtil.GetField(context, "m_CredentialsHandle");
                var credentialsHandleHandleOriginal = ReflectUtil.GetField(credentialsHandle, "_handle");
                NativeApi.SSPIHandle credentialsHandleHandle = default(NativeApi.SSPIHandle);
                credentialsHandleHandle.HandleHi = (IntPtr)ReflectUtil.GetField(credentialsHandleHandleOriginal, "HandleHi");
                credentialsHandleHandle.HandleLo = (IntPtr)ReflectUtil.GetField(credentialsHandleHandleOriginal, "HandleLo");

                int bufferSize = 1;
                NativeApi.SecurityBufferDescriptor securityBufferDescriptor = new NativeApi.SecurityBufferDescriptor(bufferSize);
                NativeApi.SecurityBufferStruct[] unmanagedBuffer = new NativeApi.SecurityBufferStruct[bufferSize];

                fixed (NativeApi.SecurityBufferStruct* ptr = unmanagedBuffer)
                fixed (void* workArrayPtr = workArray)
                {
                    securityBufferDescriptor.UnmanagedPointer = (void*)ptr;

                    unmanagedBuffer[0].token = (IntPtr)workArrayPtr;
                    unmanagedBuffer[0].count = workArray.Length;
                    unmanagedBuffer[0].type = NativeApi.BufferType.Token;

                    NativeApi.SecurityStatus status;
                    status = (NativeApi.SecurityStatus)NativeApi.ApplyControlToken(ref securityContextHandle, securityBufferDescriptor);
                    if (status == NativeApi.SecurityStatus.OK)
                    {
                        unmanagedBuffer[0].token = IntPtr.Zero;
                        unmanagedBuffer[0].count = 0;
                        unmanagedBuffer[0].type = NativeApi.BufferType.Token;

                        NativeApi.SSPIHandle contextHandleOut = default(NativeApi.SSPIHandle);
                        NativeApi.ContextFlags outflags = NativeApi.ContextFlags.Zero;
                        long ts = 0;

                        var inflags = NativeApi.ContextFlags.SequenceDetect |
                                    NativeApi.ContextFlags.ReplayDetect |
                                    NativeApi.ContextFlags.Confidentiality |
                                    NativeApi.ContextFlags.AcceptExtendedError |
                                    NativeApi.ContextFlags.AllocateMemory |
                                    NativeApi.ContextFlags.InitStream;

                        if (isServer)
                        {
                            status = (NativeApi.SecurityStatus)NativeApi.AcceptSecurityContext(ref credentialsHandleHandle, ref securityContextHandle, null,
                                inflags, NativeApi.Endianness.Native, ref contextHandleOut, securityBufferDescriptor, ref outflags, out ts);
                        }
                        else
                        {
                            status = (NativeApi.SecurityStatus)NativeApi.InitializeSecurityContextW(ref credentialsHandleHandle, ref securityContextHandle, null,
                                inflags, 0, NativeApi.Endianness.Native, null, 0, ref contextHandleOut, securityBufferDescriptor, ref outflags, out ts);
                        }
                        if (status == NativeApi.SecurityStatus.OK)
                        {
                            byte[] resultArr = new byte[unmanagedBuffer[0].count];
                            Marshal.Copy(unmanagedBuffer[0].token, resultArr, 0, resultArr.Length);
                            Marshal.FreeCoTaskMem(unmanagedBuffer[0].token);
                            result = resultArr;
                            resultSz = resultArr.Length;
                        }
                        else
                        {
                            throw new InvalidOperationException(string.Format("AcceptSecurityContext/InitializeSecurityContextW returned [{0}] during CloseNotify.", status));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("ApplyControlToken returned [{0}] during CloseNotify.", status));
                    }
                }

                var innerStream = (Stream)ReflectUtil.GetProperty(sslstate, "InnerStream");
                innerStream.Write(result, 0, resultSz);
            }
        }
    }

    public static class ReflectUtil
    {
        public static object GetField(object obj, string fieldName)
        {
            var tp = obj.GetType();
            var info = GetAllFields(tp)
                .Where(f => f.Name == fieldName).Single();
            return info.GetValue(obj);
        }
        public static void SetField(object obj, string fieldName, object value)
        {
            var tp = obj.GetType();
            var info = GetAllFields(tp)
                .Where(f => f.Name == fieldName).Single();
            info.SetValue(obj, value);
        }
        public static object GetStaticField(Assembly assembly, string typeName, string fieldName)
        {
            var tp = assembly.GetType(typeName);
            var info = GetAllFields(tp)
                .Where(f => f.IsStatic)
                .Where(f => f.Name == fieldName).Single();
            return info.GetValue(null);
        }

        public static object GetProperty(object obj, string propertyName)
        {
            var tp = obj.GetType();
            var info = GetAllProperties(tp)
                .Where(f => f.Name == propertyName).Single();
            return info.GetValue(obj, null);
        }
        public static object CallMethod(object obj, string methodName, params object[] prm)
        {
            var tp = obj.GetType();
            var info = GetAllMethods(tp)
                .Where(f => f.Name == methodName && f.GetParameters().Length == prm.Length).Single();
            object rez = info.Invoke(obj, prm);
            return rez;
        }
        public static object NewInstance(Assembly assembly, string typeName, params object[] prm)
        {
            var tp = assembly.GetType(typeName);
            var info = tp.GetConstructors()
                .Where(f => f.GetParameters().Length == prm.Length).Single();
            object rez = info.Invoke(prm);
            return rez;
        }
        public static object InvokeStaticMethod(Assembly assembly, string typeName, string methodName, params object[] prm)
        {
            var tp = assembly.GetType(typeName);
            var info = GetAllMethods(tp)
                .Where(f => f.IsStatic)
                .Where(f => f.Name == methodName && f.GetParameters().Length == prm.Length).Single();
            object rez = info.Invoke(null, prm);
            return rez;
        }
        public static object GetEnumValue(Assembly assembly, string typeName, int value)
        {
            var tp = assembly.GetType(typeName);
            object rez = Enum.ToObject(tp, value);
            return rez;
        }

        private static IEnumerable<FieldInfo> GetAllFields(Type t)
        {
            if (t == null)
                return Enumerable.Empty<FieldInfo>();

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Static | BindingFlags.Instance |
                                 BindingFlags.DeclaredOnly;
            return t.GetFields(flags).Concat(GetAllFields(t.BaseType));
        }
        private static IEnumerable<PropertyInfo> GetAllProperties(Type t)
        {
            if (t == null)
                return Enumerable.Empty<PropertyInfo>();

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Static | BindingFlags.Instance |
                                 BindingFlags.DeclaredOnly;
            return t.GetProperties(flags).Concat(GetAllProperties(t.BaseType));
        }
        private static IEnumerable<MethodInfo> GetAllMethods(Type t)
        {
            if (t == null)
                return Enumerable.Empty<MethodInfo>();

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Static | BindingFlags.Instance |
                                 BindingFlags.DeclaredOnly;
            return t.GetMethods(flags).Concat(GetAllMethods(t.BaseType));
        }
    }
}
