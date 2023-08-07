namespace demo_sdk_hikvision
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DropdownItems_pdv = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_disp_mant_cerrado = new System.Windows.Forms.Button();
            this.btn_disp_abrir = new System.Windows.Forms.Button();
            this.btn_disp_mant_abierto = new System.Windows.Forms.Button();
            this.btn_disp_cerrar = new System.Windows.Forms.Button();
            this.btn_disp_impiar = new System.Windows.Forms.Button();
            this.data_dispositivos = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.puntodeventa_disp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.direccion_ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.puerto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contrasena = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_disp_agregar = new System.Windows.Forms.Button();
            this.btn_disp_eliminar = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_disp_contrasena = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_disp_usuario = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txt_disp_puerto = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_disp_direcc_ip = new System.Windows.Forms.TextBox();
            this.groupBox_usuarios = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_DeleteUser = new System.Windows.Forms.Button();
            this.DataGridView_Usuarios = new System.Windows.Forms.DataGridView();
            this.id_usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no_tarjeta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cedula_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.puntodeventa_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.huella = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_find_user = new System.Windows.Forms.Button();
            this.txt_search_user = new System.Windows.Forms.TextBox();
            this.button_AddUser = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_evento_tipo_secundario = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_SincNewdevice = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_evento_fecha_fin = new System.Windows.Forms.DateTimePicker();
            this.txt_evento_fecha_inicio = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_evento_tipo_principal = new System.Windows.Forms.ComboBox();
            this.btn_eventos_mostrar = new System.Windows.Forms.Button();
            this.group_log = new System.Windows.Forms.GroupBox();
            this.data_user_add = new System.Windows.Forms.DataGridView();
            this.id_usuario_add = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.process = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no_tarjeta_add = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_add = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cedula_add = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fingerprint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkedListBox_Processes = new System.Windows.Forms.CheckedListBox();
            this.btn_SincOnedevice = new System.Windows.Forms.Button();
            this.btn_SincAlldevice = new System.Windows.Forms.Button();
            this.btn_usuario_eliminar = new System.Windows.Forms.Button();
            this.timer_no_tarjeta = new System.Windows.Forms.Timer(this.components);
            this.timer_barra = new System.Windows.Forms.Timer(this.components);
            this.toolTip_MessageNewDevice = new System.Windows.Forms.ToolTip(this.components);
            this.variablesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.variablesBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.variablesBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.variablesBindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_dispositivos)).BeginInit();
            this.groupBox_usuarios.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Usuarios)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.group_log.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_user_add)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.variablesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.variablesBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.variablesBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.variablesBindingSource3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.DropdownItems_pdv);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btn_disp_impiar);
            this.groupBox1.Controls.Add(this.data_dispositivos);
            this.groupBox1.Controls.Add(this.btn_disp_agregar);
            this.groupBox1.Controls.Add(this.btn_disp_eliminar);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txt_disp_contrasena);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txt_disp_usuario);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txt_disp_puerto);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txt_disp_direcc_ip);
            this.groupBox1.Location = new System.Drawing.Point(12, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 764);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dispositivos";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Descripción";
            // 
            // DropdownItems_pdv
            // 
            this.DropdownItems_pdv.FormattingEnabled = true;
            this.DropdownItems_pdv.Location = new System.Drawing.Point(12, 35);
            this.DropdownItems_pdv.Name = "DropdownItems_pdv";
            this.DropdownItems_pdv.Size = new System.Drawing.Size(326, 26);
            this.DropdownItems_pdv.TabIndex = 15;
            this.DropdownItems_pdv.DropDown += new System.EventHandler(this.DropdownItems_pdv_DropDown);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_disp_mant_cerrado);
            this.groupBox3.Controls.Add(this.btn_disp_abrir);
            this.groupBox3.Controls.Add(this.btn_disp_mant_abierto);
            this.groupBox3.Controls.Add(this.btn_disp_cerrar);
            this.groupBox3.Location = new System.Drawing.Point(17, 668);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(326, 103);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Control de puerta";
            // 
            // btn_disp_mant_cerrado
            // 
            this.btn_disp_mant_cerrado.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_disp_mant_cerrado.Location = new System.Drawing.Point(168, 55);
            this.btn_disp_mant_cerrado.Name = "btn_disp_mant_cerrado";
            this.btn_disp_mant_cerrado.Size = new System.Drawing.Size(146, 28);
            this.btn_disp_mant_cerrado.TabIndex = 66;
            this.btn_disp_mant_cerrado.Text = "Mantener cerrado";
            this.btn_disp_mant_cerrado.UseVisualStyleBackColor = false;
            this.btn_disp_mant_cerrado.Click += new System.EventHandler(this.btn_disp_mant_cerrado_Click);
            // 
            // btn_disp_abrir
            // 
            this.btn_disp_abrir.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_disp_abrir.Location = new System.Drawing.Point(12, 21);
            this.btn_disp_abrir.Name = "btn_disp_abrir";
            this.btn_disp_abrir.Size = new System.Drawing.Size(146, 28);
            this.btn_disp_abrir.TabIndex = 15;
            this.btn_disp_abrir.Text = "Abrir\r\n";
            this.btn_disp_abrir.UseVisualStyleBackColor = false;
            this.btn_disp_abrir.Click += new System.EventHandler(this.btn_disp_abrir_Click);
            // 
            // btn_disp_mant_abierto
            // 
            this.btn_disp_mant_abierto.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_disp_mant_abierto.Location = new System.Drawing.Point(12, 55);
            this.btn_disp_mant_abierto.Name = "btn_disp_mant_abierto";
            this.btn_disp_mant_abierto.Size = new System.Drawing.Size(146, 28);
            this.btn_disp_mant_abierto.TabIndex = 65;
            this.btn_disp_mant_abierto.Text = "Mantener abierto";
            this.btn_disp_mant_abierto.UseVisualStyleBackColor = false;
            this.btn_disp_mant_abierto.Click += new System.EventHandler(this.btn_disp_mant_abierto_Click);
            // 
            // btn_disp_cerrar
            // 
            this.btn_disp_cerrar.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_disp_cerrar.Location = new System.Drawing.Point(168, 21);
            this.btn_disp_cerrar.Name = "btn_disp_cerrar";
            this.btn_disp_cerrar.Size = new System.Drawing.Size(146, 28);
            this.btn_disp_cerrar.TabIndex = 14;
            this.btn_disp_cerrar.Text = "Cerrar\r\n";
            this.btn_disp_cerrar.UseVisualStyleBackColor = false;
            this.btn_disp_cerrar.Click += new System.EventHandler(this.btn_disp_cerrar_Click);
            // 
            // btn_disp_impiar
            // 
            this.btn_disp_impiar.Location = new System.Drawing.Point(124, 154);
            this.btn_disp_impiar.Name = "btn_disp_impiar";
            this.btn_disp_impiar.Size = new System.Drawing.Size(101, 28);
            this.btn_disp_impiar.TabIndex = 13;
            this.btn_disp_impiar.Text = "Limpiar";
            this.btn_disp_impiar.UseVisualStyleBackColor = true;
            this.btn_disp_impiar.Click += new System.EventHandler(this.btn_disp_impiar_Click);
            // 
            // data_dispositivos
            // 
            this.data_dispositivos.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.data_dispositivos.AllowUserToAddRows = false;
            this.data_dispositivos.AllowUserToDeleteRows = false;
            this.data_dispositivos.AllowUserToResizeRows = false;
            this.data_dispositivos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.data_dispositivos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_dispositivos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.descripcion,
            this.puntodeventa_disp,
            this.direccion_ip,
            this.puerto,
            this.usuario,
            this.contrasena,
            this.estatus});
            this.data_dispositivos.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.data_dispositivos.Location = new System.Drawing.Point(12, 208);
            this.data_dispositivos.Name = "data_dispositivos";
            this.data_dispositivos.ReadOnly = true;
            this.data_dispositivos.RowHeadersVisible = false;
            this.data_dispositivos.RowHeadersWidth = 51;
            this.data_dispositivos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.data_dispositivos.Size = new System.Drawing.Size(326, 435);
            this.data_dispositivos.TabIndex = 12;
            this.data_dispositivos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_dispositivos_CellClick);
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 125;
            // 
            // descripcion
            // 
            this.descripcion.HeaderText = "Descipción";
            this.descripcion.MinimumWidth = 6;
            this.descripcion.Name = "descripcion";
            this.descripcion.ReadOnly = true;
            this.descripcion.Width = 180;
            // 
            // puntodeventa_disp
            // 
            this.puntodeventa_disp.HeaderText = "Punto de venta";
            this.puntodeventa_disp.MinimumWidth = 6;
            this.puntodeventa_disp.Name = "puntodeventa_disp";
            this.puntodeventa_disp.ReadOnly = true;
            this.puntodeventa_disp.Width = 125;
            // 
            // direccion_ip
            // 
            this.direccion_ip.HeaderText = "Dirección IP";
            this.direccion_ip.MinimumWidth = 6;
            this.direccion_ip.Name = "direccion_ip";
            this.direccion_ip.ReadOnly = true;
            this.direccion_ip.Width = 125;
            // 
            // puerto
            // 
            this.puerto.HeaderText = "Puerto";
            this.puerto.MinimumWidth = 6;
            this.puerto.Name = "puerto";
            this.puerto.ReadOnly = true;
            this.puerto.Width = 125;
            // 
            // usuario
            // 
            this.usuario.HeaderText = "Usuario";
            this.usuario.MinimumWidth = 6;
            this.usuario.Name = "usuario";
            this.usuario.ReadOnly = true;
            this.usuario.Width = 125;
            // 
            // contrasena
            // 
            this.contrasena.HeaderText = "Contraseña";
            this.contrasena.MinimumWidth = 6;
            this.contrasena.Name = "contrasena";
            this.contrasena.ReadOnly = true;
            this.contrasena.Visible = false;
            this.contrasena.Width = 125;
            // 
            // estatus
            // 
            this.estatus.HeaderText = "Estatus";
            this.estatus.MinimumWidth = 6;
            this.estatus.Name = "estatus";
            this.estatus.ReadOnly = true;
            this.estatus.Width = 125;
            // 
            // btn_disp_agregar
            // 
            this.btn_disp_agregar.Location = new System.Drawing.Point(231, 154);
            this.btn_disp_agregar.Name = "btn_disp_agregar";
            this.btn_disp_agregar.Size = new System.Drawing.Size(107, 28);
            this.btn_disp_agregar.TabIndex = 11;
            this.btn_disp_agregar.Text = "Agregar";
            this.btn_disp_agregar.UseVisualStyleBackColor = true;
            this.btn_disp_agregar.Click += new System.EventHandler(this.btn_disp_agregar_Click);
            // 
            // btn_disp_eliminar
            // 
            this.btn_disp_eliminar.Enabled = false;
            this.btn_disp_eliminar.Location = new System.Drawing.Point(12, 154);
            this.btn_disp_eliminar.Name = "btn_disp_eliminar";
            this.btn_disp_eliminar.Size = new System.Drawing.Size(106, 28);
            this.btn_disp_eliminar.TabIndex = 10;
            this.btn_disp_eliminar.Text = "Eliminar";
            this.btn_disp_eliminar.UseVisualStyleBackColor = true;
            this.btn_disp_eliminar.Click += new System.EventHandler(this.btn_disp_eliminar_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(140, 103);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(93, 19);
            this.label16.TabIndex = 9;
            this.label16.Text = "Contraseña";
            // 
            // txt_disp_contrasena
            // 
            this.txt_disp_contrasena.Location = new System.Drawing.Point(143, 120);
            this.txt_disp_contrasena.MaxLength = 16;
            this.txt_disp_contrasena.Name = "txt_disp_contrasena";
            this.txt_disp_contrasena.PasswordChar = '■';
            this.txt_disp_contrasena.Size = new System.Drawing.Size(195, 26);
            this.txt_disp_contrasena.TabIndex = 8;
            this.txt_disp_contrasena.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_disp_contrasena.UseSystemPasswordChar = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 103);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 19);
            this.label15.TabIndex = 7;
            this.label15.Text = "Usuario";
            // 
            // txt_disp_usuario
            // 
            this.txt_disp_usuario.Location = new System.Drawing.Point(12, 120);
            this.txt_disp_usuario.MaxLength = 16;
            this.txt_disp_usuario.Name = "txt_disp_usuario";
            this.txt_disp_usuario.Size = new System.Drawing.Size(125, 26);
            this.txt_disp_usuario.TabIndex = 6;
            this.txt_disp_usuario.Text = "admin";
            this.txt_disp_usuario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(253, 62);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 19);
            this.label14.TabIndex = 5;
            this.label14.Text = "Puerto";
            // 
            // txt_disp_puerto
            // 
            this.txt_disp_puerto.Location = new System.Drawing.Point(256, 78);
            this.txt_disp_puerto.MaxLength = 16;
            this.txt_disp_puerto.Name = "txt_disp_puerto";
            this.txt_disp_puerto.Size = new System.Drawing.Size(82, 26);
            this.txt_disp_puerto.TabIndex = 4;
            this.txt_disp_puerto.Text = "8000";
            this.txt_disp_puerto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 19);
            this.label13.TabIndex = 3;
            this.label13.Text = "Dirección IP";
            // 
            // txt_disp_direcc_ip
            // 
            this.txt_disp_direcc_ip.Location = new System.Drawing.Point(12, 78);
            this.txt_disp_direcc_ip.MaxLength = 16;
            this.txt_disp_direcc_ip.Name = "txt_disp_direcc_ip";
            this.txt_disp_direcc_ip.Size = new System.Drawing.Size(238, 26);
            this.txt_disp_direcc_ip.TabIndex = 2;
            this.txt_disp_direcc_ip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox_usuarios
            // 
            this.groupBox_usuarios.Controls.Add(this.groupBox5);
            this.groupBox_usuarios.Controls.Add(this.groupBox4);
            this.groupBox_usuarios.Controls.Add(this.group_log);
            this.groupBox_usuarios.Location = new System.Drawing.Point(370, 1);
            this.groupBox_usuarios.Name = "groupBox_usuarios";
            this.groupBox_usuarios.Size = new System.Drawing.Size(970, 778);
            this.groupBox_usuarios.TabIndex = 64;
            this.groupBox_usuarios.TabStop = false;
            this.groupBox_usuarios.Text = "Usuarios";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.groupBox5.Controls.Add(this.button_DeleteUser);
            this.groupBox5.Controls.Add(this.DataGridView_Usuarios);
            this.groupBox5.Controls.Add(this.btn_find_user);
            this.groupBox5.Controls.Add(this.txt_search_user);
            this.groupBox5.Controls.Add(this.button_AddUser);
            this.groupBox5.Location = new System.Drawing.Point(6, 19);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(477, 611);
            this.groupBox5.TabIndex = 78;
            this.groupBox5.TabStop = false;
            // 
            // button_DeleteUser
            // 
            this.button_DeleteUser.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button_DeleteUser.Location = new System.Drawing.Point(250, 572);
            this.button_DeleteUser.Name = "button_DeleteUser";
            this.button_DeleteUser.Size = new System.Drawing.Size(194, 30);
            this.button_DeleteUser.TabIndex = 81;
            this.button_DeleteUser.Text = "Eliminar del dispositivo";
            this.button_DeleteUser.UseVisualStyleBackColor = false;
            this.button_DeleteUser.Click += new System.EventHandler(this.button_DeleteUser_Click);
            // 
            // DataGridView_Usuarios
            // 
            this.DataGridView_Usuarios.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.DataGridView_Usuarios.AllowUserToAddRows = false;
            this.DataGridView_Usuarios.AllowUserToDeleteRows = false;
            this.DataGridView_Usuarios.AllowUserToResizeColumns = false;
            this.DataGridView_Usuarios.AllowUserToResizeRows = false;
            this.DataGridView_Usuarios.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DataGridView_Usuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_Usuarios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_usuario,
            this.no_tarjeta,
            this.nombre,
            this.cedula_user,
            this.puntodeventa_user,
            this.huella});
            this.DataGridView_Usuarios.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DataGridView_Usuarios.Location = new System.Drawing.Point(6, 48);
            this.DataGridView_Usuarios.Name = "DataGridView_Usuarios";
            this.DataGridView_Usuarios.ReadOnly = true;
            this.DataGridView_Usuarios.RowHeadersVisible = false;
            this.DataGridView_Usuarios.RowHeadersWidth = 51;
            this.DataGridView_Usuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView_Usuarios.Size = new System.Drawing.Size(464, 515);
            this.DataGridView_Usuarios.TabIndex = 12;
            this.DataGridView_Usuarios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_usuarios_CellClick);
            // 
            // id_usuario
            // 
            this.id_usuario.HeaderText = "ID";
            this.id_usuario.MinimumWidth = 6;
            this.id_usuario.Name = "id_usuario";
            this.id_usuario.ReadOnly = true;
            this.id_usuario.Visible = false;
            this.id_usuario.Width = 160;
            // 
            // no_tarjeta
            // 
            this.no_tarjeta.HeaderText = "No. Tarjeta";
            this.no_tarjeta.MinimumWidth = 6;
            this.no_tarjeta.Name = "no_tarjeta";
            this.no_tarjeta.ReadOnly = true;
            this.no_tarjeta.Width = 130;
            // 
            // nombre
            // 
            this.nombre.HeaderText = "Nombre";
            this.nombre.MinimumWidth = 6;
            this.nombre.Name = "nombre";
            this.nombre.ReadOnly = true;
            this.nombre.Width = 170;
            // 
            // cedula_user
            // 
            this.cedula_user.HeaderText = "Cedula";
            this.cedula_user.MinimumWidth = 6;
            this.cedula_user.Name = "cedula_user";
            this.cedula_user.ReadOnly = true;
            this.cedula_user.Width = 130;
            // 
            // puntodeventa_user
            // 
            this.puntodeventa_user.HeaderText = "Punto de venta";
            this.puntodeventa_user.MinimumWidth = 6;
            this.puntodeventa_user.Name = "puntodeventa_user";
            this.puntodeventa_user.ReadOnly = true;
            this.puntodeventa_user.Width = 125;
            // 
            // huella
            // 
            this.huella.HeaderText = "Huella";
            this.huella.MinimumWidth = 6;
            this.huella.Name = "huella";
            this.huella.ReadOnly = true;
            this.huella.Width = 125;
            // 
            // btn_find_user
            // 
            this.btn_find_user.Location = new System.Drawing.Point(316, 15);
            this.btn_find_user.Name = "btn_find_user";
            this.btn_find_user.Size = new System.Drawing.Size(128, 26);
            this.btn_find_user.TabIndex = 80;
            this.btn_find_user.Text = "Buscar usuario";
            this.btn_find_user.UseVisualStyleBackColor = true;
            this.btn_find_user.Click += new System.EventHandler(this.btn_find_user_Click);
            // 
            // txt_search_user
            // 
            this.txt_search_user.Location = new System.Drawing.Point(14, 16);
            this.txt_search_user.Name = "txt_search_user";
            this.txt_search_user.Size = new System.Drawing.Size(279, 26);
            this.txt_search_user.TabIndex = 79;
            // 
            // button_AddUser
            // 
            this.button_AddUser.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button_AddUser.Location = new System.Drawing.Point(50, 572);
            this.button_AddUser.Name = "button_AddUser";
            this.button_AddUser.Size = new System.Drawing.Size(194, 30);
            this.button_AddUser.TabIndex = 75;
            this.button_AddUser.Text = "Agregar al dispositivo";
            this.button_AddUser.UseVisualStyleBackColor = false;
            this.button_AddUser.Click += new System.EventHandler(this.button_AddUser_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_evento_tipo_secundario);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.btn_SincNewdevice);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.txt_evento_fecha_fin);
            this.groupBox4.Controls.Add(this.txt_evento_fecha_inicio);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.txt_evento_tipo_principal);
            this.groupBox4.Controls.Add(this.btn_eventos_mostrar);
            this.groupBox4.Location = new System.Drawing.Point(6, 645);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(477, 125);
            this.groupBox4.TabIndex = 76;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Eventos";
            // 
            // txt_evento_tipo_secundario
            // 
            this.txt_evento_tipo_secundario.FormattingEnabled = true;
            this.txt_evento_tipo_secundario.Items.AddRange(new object[] {
            "All",
            "4G_MOUDLE_OFFLINE",
            "4G_MOUDLE_ONLINE",
            "AC_OFF",
            "AC_RESUME",
            "ALARMIN_ARM",
            "ALARMIN_BROKEN_CIRCUIT",
            "ALARMIN_DISARM",
            "ALARMIN_EXCEPTION",
            "ALARMIN_RESUME",
            "ALARMIN_SHORT_CIRCUIT",
            "ALARMOUT_OFF",
            "ALARMOUT_ON",
            "ALWAYS_CLOSE_BEGIN",
            "ALWAYS_CLOSE_END",
            "ALWAYS_OPEN_BEGIN",
            "ALWAYS_OPEN_END",
            "ANTI_SNEAK_FAIL",
            "AUTH_PLAN_DORMANT_FAIL",
            "AUTO_COMPLEMENT_NUMBER",
            "AUTO_RENUMBER",
            "BATTERY_ELECTRIC_LOW",
            "BATTERY_ELECTRIC_RESUME",
            "BATTERY_RESUME",
            "CAMERA_NOT_CONNECT",
            "CAMERA_RESUME",
            "CAN_BUS_EXCEPTION",
            "CAN_BUS_RESUME",
            "CARD_AND_PSW_FAIL",
            "CARD_AND_PSW_OVER_TIME",
            "CARD_AND_PSW_PASS",
            "CARD_AND_PSW_TIMEOUT",
            "CARD_ENCRYPT_VERIFY_FAIL",
            "CARD_FINGERPRINT_PASSWD_VERIFY_FAIL",
            "CARD_FINGERPRINT_PASSWD_VERIFY_PASS",
            "CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT",
            "CARD_FINGERPRINT_VERIFY_FAIL",
            "CARD_FINGERPRINT_VERIFY_PASS",
            "CARD_FINGERPRINT_VERIFY_TIMEOUT",
            "CARD_INVALID_PERIOD",
            "CARD_MAX_AUTHENTICATE_FAIL",
            "CARD_NO_RIGHT",
            "CARD_OUT_OF_DATE",
            "CARD_PLATFORM_VERIFY",
            "CARD_READER_DESMANTLE_ALARM",
            "CARD_READER_DESMANTLE_RESUME",
            "CARD_READER_OFFLINE",
            "CARD_READER_RESUME",
            "CARD_RIGHT_INPUT",
            "CARD_RIGHT_OUTTPUT",
            "CASE_SENSOR_ALARM",
            "CASE_SENSOR_RESUME",
            "CHANNEL_CONTROLLER_DESMANTLE_ALARM",
            "CHANNEL_CONTROLLER_DESMANTLE_RESUME",
            "CHANNEL_CONTROLLER_FIRE_IMPORT_ALARM",
            "CHANNEL_CONTROLLER_FIRE_IMPORT_RESUME",
            "CHANNEL_CONTROLLER_OFF",
            "CHANNEL_CONTROLLER_RESUME",
            "CLIMBING_OVER_GATE",
            "COM_NOT_CONNECT",
            "COM_RESUME",
            "COMBINED_VERIFY_PASS",
            "COMBINED_VERIFY_TIMEOUT",
            "DEV_POWER_OFF",
            "DEV_POWER_ON",
            "DEVICE_NOT_AUTHORIZE",
            "DISTRACT_CONTROLLER_ALARM",
            "DISTRACT_CONTROLLER_OFFLINE",
            "DISTRACT_CONTROLLER_ONLINE",
            "DISTRACT_CONTROLLER_RESUME",
            "DOOR_BUTTON_PRESS",
            "DOOR_BUTTON_RELEASE",
            "DOOR_CLOSE_NORMAL",
            "DOOR_OPEN_ABNORMAL",
            "DOOR_OPEN_NORMAL",
            "DOOR_OPEN_OR_DORMANT_FAIL",
            "DOOR_OPEN_OR_DORMANT_LINKAGE_OPEN_FAIL",
            "DOOR_OPEN_OR_DORMANT_OPEN_FAIL",
            "DOOR_OPEN_TIMEOUT",
            "DOORBELL_RINGING",
            "DROP_ARM_BLOCK",
            "DROP_ARM_BLOCK_RESUME",
            "EMERGENCY_BUTTON_RESUME",
            "EMERGENCY_BUTTON_TRIGGER",
            "EMPLOYEE_NO_NOT_EXIST",
            "FACE_IMAGE_QUALITY_LOW",
            "FINGE_RPRINT_QUALITY_LOW",
            "FINGER_PRINT_MODULE_NOT_CONNECT",
            "FINGER_PRINT_MODULE_RESUME",
            "FINGERPRINT_COMPARE_FAIL",
            "FINGERPRINT_COMPARE_PASS",
            "FINGERPRINT_INEXISTENCE",
            "FINGERPRINT_PASSWD_VERIFY_FAIL",
            "FINGERPRINT_PASSWD_VERIFY_PASS",
            "FINGERPRINT_PASSWD_VERIFY_TIMEOUT",
            "FIRE_BUTTON_RESUME",
            "FIRE_BUTTON_TRIGGER",
            "FIRE_IMPORT_BROKEN_CIRCUIT",
            "FIRE_IMPORT_RESUME",
            "FLASH_ABNORMAL",
            "FORCE_ACCESS",
            "FREE_GATE_PASS_NOT_AUTH",
            "GATE_TEMPERATURE_OVERRUN",
            "HOST_DESMANTLE_ALARM",
            "HOST_DESMANTLE_RESUME",
            "ID_CARD_READER_NOT_CONNECT",
            "ID_CARD_READER_RESUME",
            "ILLEGAL_MESSAGE",
            "INDICATOR_LIGHT_OFF",
            "INDICATOR_LIGHT_RESUME",
            "INTERLOCK_DOOR_NOT_CLOSE",
            "INTRUSION_ALARM",
            "INVALID_CARD",
            "INVALID_MULTI_VERIFY_PERIOD",
            "IR_ADAPTOR_COMM_EXCEPTION",
            "IR_ADAPTOR_COMM_RESUME",
            "IR_EMITTER_EXCEPTION",
            "IR_EMITTER_RESUME",
            "LAMP_BOARD_COMM_EXCEPTION",
            "LAMP_BOARD_COMM_RESUME",
            "LEADER_CARD_OPEN_BEGIN",
            "LEADER_CARD_OPEN_END",
            "LEGAL_CARD_PASS",
            "LEGAL_EVENT_NEARLY_FULL",
            "LEGAL_MESSAGE",
            "LINKAGE_CAPTURE_PIC",
            "LOCAL_CONTROL_NET_BROKEN",
            "LOCAL_CONTROL_NET_RSUME",
            "LOCAL_CONTROL_OFFLINE",
            "LOCAL_CONTROL_RESUME",
            "LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN",
            "LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME",
            "LOCAL_FACE_MODELING_FAIL",
            "LOCAL_LOGIN_LOCK",
            "LOCAL_LOGIN_UNLOCK",
            "LOCAL_RESTORE_CFG",
            "LOCAL_UPGRADE",
            "LOCAL_USB_UPGRADE",
            "LOCK_CLOSE",
            "LOCK_OPEN",
            "LOW_BATTERY",
            "MAC_DETECT",
            "MAINTENANCE_BUTTON_RESUME",
            "MAINTENANCE_BUTTON_TRIGGER",
            "MASTER_RS485_LOOPNODE_BROKEN",
            "MASTER_RS485_LOOPNODE_RESUME",
            "MINOR_REMOTE_ARM",
            "MOD_GPRS_REPORT_PARAM",
            "MOD_NET_REPORT_CFG",
            "MOD_REPORT_GROUP_PARAM",
            "MOTOR_SENSOR_EXCEPTION",
            "MULTI_VERIFY_NEED_REMOTE_OPEN",
            "MULTI_VERIFY_REMOTE_RIGHT_FAIL",
            "MULTI_VERIFY_REPEAT_VERIFY",
            "MULTI_VERIFY_SUCCESS",
            "MULTI_VERIFY_SUPER_RIGHT_FAIL",
            "MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS",
            "MULTI_VERIFY_TIMEOUT",
            "NET_BROKEN",
            "NET_RESUME",
            "NORMAL_CFGFILE_INPUT",
            "NORMAL_CFGFILE_OUTTPUT",
            "NOT_BELONG_MULTI_GROUP",
            "NTP_CHECK_TIME",
            "OFFLINE_ECENT_NEARLY_FULL",
            "PASSING_TIMEOUT",
            "PASSWORD_MISMATCH",
            "PEOPLE_AND_ID_CARD_DEVICE_OFFLINE",
            "PEOPLE_AND_ID_CARD_DEVICE_ONLINE",
            "POS_END_ALARM",
            "POS_START_ALARM",
            "PRINTER_OFFLINE",
            "PRINTER_ONLINE",
            "PRINTER_OUT_OF_PAPER",
            "REMOTE_ACTUAL_GUARD",
            "REMOTE_ACTUAL_UNGUARD",
            "REMOTE_ALARMOUT_CLOSE_MAN",
            "REMOTE_ALARMOUT_OPEN_MAN",
            "REMOTE_ALWAYS_CLOSE",
            "REMOTE_ALWAYS_OPEN",
            "REMOTE_CAPTURE_PIC",
            "REMOTE_CFGFILE_INTPUT",
            "REMOTE_CFGFILE_OUTPUT",
            "REMOTE_CHECK_TIME",
            "REMOTE_CLEAR_CARD",
            "REMOTE_CLOSE_DOOR",
            "REMOTE_CONTROL_ALWAYS_OPEN_DOOR",
            "REMOTE_CONTROL_CLOSE_DOOR",
            "REMOTE_CONTROL_NOT_CODE_OPER_FAILED",
            "REMOTE_CONTROL_OPEN_DOOR",
            "REMOTE_DISARM",
            "REMOTE_HOUSEHOLD_CALL_LADDER",
            "REMOTE_LOGIN",
            "REMOTE_LOGOUT",
            "REMOTE_OPEN_DOOR",
            "REMOTE_REBOOT",
            "REMOTE_RESTORE_CFG",
            "REMOTE_UPGRADE",
            "REMOTE_VISITOR_CALL_LADDER",
            "REVERSE_ACCESS",
            "RS485_DEVICE_ABNORMAL",
            "RS485_DEVICE_REVERT",
            "SD_CARD_FULL",
            "SECURITY_MODULE_DESMANTLE_ALARM",
            "SECURITY_MODULE_DESMANTLE_RESUME",
            "SECURITY_MODULE_OFF",
            "SECURITY_MODULE_RESUME",
            "STAY_EVENT",
            "STRESS_ALARM",
            "SUBMARINEBACK_COMM_BREAK",
            "SUBMARINEBACK_COMM_RESUME",
            "SUBMARINEBACK_REPLY_FAIL",
            "TRAILING",
            "UNLOCK_PASSWORD_OPEN_DOOR",
            "VERIFY_MODE_MISMATCH",
            "WATCH_DOG_RESET",
            "FACE_VERIFY_PASS"});
            this.txt_evento_tipo_secundario.Location = new System.Drawing.Point(136, 94);
            this.txt_evento_tipo_secundario.Name = "txt_evento_tipo_secundario";
            this.txt_evento_tipo_secundario.Size = new System.Drawing.Size(169, 26);
            this.txt_evento_tipo_secundario.TabIndex = 72;
            this.txt_evento_tipo_secundario.Text = "All";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 19);
            this.label5.TabIndex = 71;
            this.label5.Text = "Tipo (secundario)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 19);
            this.label4.TabIndex = 70;
            this.label4.Text = "Tipo (principal)";
            // 
            // btn_SincNewdevice
            // 
            this.btn_SincNewdevice.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_SincNewdevice.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btn_SincNewdevice.Location = new System.Drawing.Point(136, -29);
            this.btn_SincNewdevice.Name = "btn_SincNewdevice";
            this.btn_SincNewdevice.Size = new System.Drawing.Size(265, 48);
            this.btn_SincNewdevice.TabIndex = 68;
            this.btn_SincNewdevice.Text = "Aplicar a dispositivo nuevo";
            this.btn_SincNewdevice.UseVisualStyleBackColor = false;
            this.btn_SincNewdevice.Click += new System.EventHandler(this.btn_SincNewdevice_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(229, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 19);
            this.label3.TabIndex = 69;
            this.label3.Text = "Hasta";
            // 
            // txt_evento_fecha_fin
            // 
            this.txt_evento_fecha_fin.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.txt_evento_fecha_fin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txt_evento_fecha_fin.Location = new System.Drawing.Point(279, 22);
            this.txt_evento_fecha_fin.Name = "txt_evento_fecha_fin";
            this.txt_evento_fecha_fin.Size = new System.Drawing.Size(192, 26);
            this.txt_evento_fecha_fin.TabIndex = 68;
            // 
            // txt_evento_fecha_inicio
            // 
            this.txt_evento_fecha_inicio.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.txt_evento_fecha_inicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txt_evento_fecha_inicio.Location = new System.Drawing.Point(63, 22);
            this.txt_evento_fecha_inicio.Name = "txt_evento_fecha_inicio";
            this.txt_evento_fecha_inicio.Size = new System.Drawing.Size(162, 26);
            this.txt_evento_fecha_inicio.TabIndex = 67;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 19);
            this.label2.TabIndex = 61;
            this.label2.Text = "Desde";
            // 
            // txt_evento_tipo_principal
            // 
            this.txt_evento_tipo_principal.FormattingEnabled = true;
            this.txt_evento_tipo_principal.Items.AddRange(new object[] {
            "All",
            "Alarm",
            "Exception",
            "Operation",
            "Event"});
            this.txt_evento_tipo_principal.Location = new System.Drawing.Point(136, 62);
            this.txt_evento_tipo_principal.Name = "txt_evento_tipo_principal";
            this.txt_evento_tipo_principal.Size = new System.Drawing.Size(169, 26);
            this.txt_evento_tipo_principal.TabIndex = 60;
            this.txt_evento_tipo_principal.Text = "All";
            // 
            // btn_eventos_mostrar
            // 
            this.btn_eventos_mostrar.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_eventos_mostrar.Location = new System.Drawing.Point(332, 60);
            this.btn_eventos_mostrar.Name = "btn_eventos_mostrar";
            this.btn_eventos_mostrar.Size = new System.Drawing.Size(123, 60);
            this.btn_eventos_mostrar.TabIndex = 15;
            this.btn_eventos_mostrar.Text = "Mostrar";
            this.btn_eventos_mostrar.UseVisualStyleBackColor = false;
            this.btn_eventos_mostrar.Click += new System.EventHandler(this.btn_eventos_mostrar_Click);
            // 
            // group_log
            // 
            this.group_log.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.group_log.Controls.Add(this.data_user_add);
            this.group_log.Controls.Add(this.groupBox2);
            this.group_log.Controls.Add(this.btn_usuario_eliminar);
            this.group_log.Location = new System.Drawing.Point(495, 19);
            this.group_log.Margin = new System.Windows.Forms.Padding(1);
            this.group_log.Name = "group_log";
            this.group_log.Size = new System.Drawing.Size(475, 745);
            this.group_log.TabIndex = 64;
            this.group_log.TabStop = false;
            this.group_log.Text = "Lista de Procesos";
            // 
            // data_user_add
            // 
            this.data_user_add.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.data_user_add.AllowUserToAddRows = false;
            this.data_user_add.AllowUserToDeleteRows = false;
            this.data_user_add.AllowUserToResizeColumns = false;
            this.data_user_add.AllowUserToResizeRows = false;
            this.data_user_add.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.data_user_add.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_user_add.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_usuario_add,
            this.process,
            this.no_tarjeta_add,
            this.nombre_add,
            this.cedula_add,
            this.fingerprint});
            this.data_user_add.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.data_user_add.Location = new System.Drawing.Point(6, 25);
            this.data_user_add.Name = "data_user_add";
            this.data_user_add.ReadOnly = true;
            this.data_user_add.RowHeadersVisible = false;
            this.data_user_add.RowHeadersWidth = 51;
            this.data_user_add.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.data_user_add.Size = new System.Drawing.Size(458, 538);
            this.data_user_add.TabIndex = 74;
            // 
            // id_usuario_add
            // 
            this.id_usuario_add.HeaderText = "ID";
            this.id_usuario_add.MinimumWidth = 6;
            this.id_usuario_add.Name = "id_usuario_add";
            this.id_usuario_add.ReadOnly = true;
            this.id_usuario_add.Visible = false;
            this.id_usuario_add.Width = 160;
            // 
            // process
            // 
            this.process.HeaderText = "Proceso";
            this.process.MinimumWidth = 6;
            this.process.Name = "process";
            this.process.ReadOnly = true;
            this.process.Width = 125;
            // 
            // no_tarjeta_add
            // 
            this.no_tarjeta_add.HeaderText = "No. Tarjeta";
            this.no_tarjeta_add.MinimumWidth = 6;
            this.no_tarjeta_add.Name = "no_tarjeta_add";
            this.no_tarjeta_add.ReadOnly = true;
            this.no_tarjeta_add.Width = 130;
            // 
            // nombre_add
            // 
            this.nombre_add.HeaderText = "Nombre";
            this.nombre_add.MinimumWidth = 6;
            this.nombre_add.Name = "nombre_add";
            this.nombre_add.ReadOnly = true;
            this.nombre_add.Width = 170;
            // 
            // cedula_add
            // 
            this.cedula_add.HeaderText = "Cedula";
            this.cedula_add.MinimumWidth = 6;
            this.cedula_add.Name = "cedula_add";
            this.cedula_add.ReadOnly = true;
            this.cedula_add.Width = 130;
            // 
            // fingerprint
            // 
            this.fingerprint.HeaderText = "Huella";
            this.fingerprint.MinimumWidth = 6;
            this.fingerprint.Name = "fingerprint";
            this.fingerprint.ReadOnly = true;
            this.fingerprint.Width = 170;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox2.Controls.Add(this.checkedListBox_Processes);
            this.groupBox2.Controls.Add(this.btn_SincOnedevice);
            this.groupBox2.Controls.Add(this.btn_SincAlldevice);
            this.groupBox2.Location = new System.Drawing.Point(11, 609);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(458, 130);
            this.groupBox2.TabIndex = 73;
            this.groupBox2.TabStop = false;
            // 
            // checkedListBox_Processes
            // 
            this.checkedListBox_Processes.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.checkedListBox_Processes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox_Processes.CheckOnClick = true;
            this.checkedListBox_Processes.FormattingEnabled = true;
            this.checkedListBox_Processes.Items.AddRange(new object[] {
            "Cargar usuarios ya registrados",
            "Aplicar procesos de la lista"});
            this.checkedListBox_Processes.Location = new System.Drawing.Point(17, 17);
            this.checkedListBox_Processes.Name = "checkedListBox_Processes";
            this.checkedListBox_Processes.Size = new System.Drawing.Size(254, 42);
            this.checkedListBox_Processes.TabIndex = 73;
            // 
            // btn_SincOnedevice
            // 
            this.btn_SincOnedevice.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_SincOnedevice.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btn_SincOnedevice.Location = new System.Drawing.Point(17, 73);
            this.btn_SincOnedevice.Name = "btn_SincOnedevice";
            this.btn_SincOnedevice.Size = new System.Drawing.Size(212, 48);
            this.btn_SincOnedevice.TabIndex = 67;
            this.btn_SincOnedevice.Text = "Aplicar a dispositivo";
            this.btn_SincOnedevice.UseVisualStyleBackColor = false;
            this.btn_SincOnedevice.Click += new System.EventHandler(this.btn_SincOnedevice_Click);
            // 
            // btn_SincAlldevice
            // 
            this.btn_SincAlldevice.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_SincAlldevice.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btn_SincAlldevice.Location = new System.Drawing.Point(235, 73);
            this.btn_SincAlldevice.Name = "btn_SincAlldevice";
            this.btn_SincAlldevice.Size = new System.Drawing.Size(212, 48);
            this.btn_SincAlldevice.TabIndex = 72;
            this.btn_SincAlldevice.Text = "Aplicar a todos los dispositivos.";
            this.btn_SincAlldevice.UseVisualStyleBackColor = false;
            this.btn_SincAlldevice.Click += new System.EventHandler(this.btn_SincAlldevice_Click);
            // 
            // btn_usuario_eliminar
            // 
            this.btn_usuario_eliminar.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_usuario_eliminar.Location = new System.Drawing.Point(138, 572);
            this.btn_usuario_eliminar.Name = "btn_usuario_eliminar";
            this.btn_usuario_eliminar.Size = new System.Drawing.Size(227, 30);
            this.btn_usuario_eliminar.TabIndex = 7;
            this.btn_usuario_eliminar.Text = "Eliminar proceso de la lista ";
            this.btn_usuario_eliminar.UseVisualStyleBackColor = false;
            this.btn_usuario_eliminar.Click += new System.EventHandler(this.btn_usuario_eliminar_Click);
            // 
            // timer_no_tarjeta
            // 
            this.timer_no_tarjeta.Interval = 500;
            this.timer_no_tarjeta.Tick += new System.EventHandler(this.timer_no_tarjeta_Tick);
            // 
            // timer_barra
            // 
            this.timer_barra.Enabled = true;
            // 
            // toolTip_MessageNewDevice
            // 
            this.toolTip_MessageNewDevice.IsBalloon = true;
            this.toolTip_MessageNewDevice.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // variablesBindingSource
            // 
            this.variablesBindingSource.DataSource = typeof(demo_sdk_hikvision.Variables);
            // 
            // variablesBindingSource1
            // 
            this.variablesBindingSource1.DataSource = typeof(demo_sdk_hikvision.Variables);
            // 
            // variablesBindingSource2
            // 
            this.variablesBindingSource2.DataSource = typeof(demo_sdk_hikvision.Variables);
            // 
            // variablesBindingSource3
            // 
            this.variablesBindingSource3.DataSource = typeof(demo_sdk_hikvision.Variables);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1346, 783);
            this.Controls.Add(this.groupBox_usuarios);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio - Roanet";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_dispositivos)).EndInit();
            this.groupBox_usuarios.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Usuarios)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.group_log.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_user_add)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.variablesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.variablesBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.variablesBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.variablesBindingSource3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_disp_impiar;
        private System.Windows.Forms.DataGridView data_dispositivos;
        private System.Windows.Forms.Button btn_disp_agregar;
        private System.Windows.Forms.Button btn_disp_eliminar;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt_disp_contrasena;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txt_disp_usuario;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_disp_puerto;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_disp_direcc_ip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox_usuarios;
        private System.Windows.Forms.DataGridView DataGridView_Usuarios;
        private System.Windows.Forms.Button btn_usuario_eliminar;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_disp_mant_cerrado;
        private System.Windows.Forms.Button btn_disp_abrir;
        private System.Windows.Forms.Button btn_disp_mant_abierto;
        private System.Windows.Forms.Button btn_disp_cerrar;
        private System.Windows.Forms.GroupBox group_log;
        private System.Windows.Forms.Button btn_SincNewdevice;
        private System.Windows.Forms.Button btn_SincOnedevice;
        private System.Windows.Forms.Timer timer_no_tarjeta;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_SincAlldevice;
        private System.Windows.Forms.Button button_AddUser;
        private System.Windows.Forms.Timer timer_barra;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox txt_evento_tipo_secundario;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker txt_evento_fecha_fin;
        private System.Windows.Forms.DateTimePicker txt_evento_fecha_inicio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox txt_evento_tipo_principal;
        private System.Windows.Forms.Button btn_eventos_mostrar;
        private System.Windows.Forms.ComboBox DropdownItems_pdv;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txt_search_user;
        private System.Windows.Forms.Button btn_find_user;
        private System.Windows.Forms.BindingSource variablesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn puntodeventa_disp;
        private System.Windows.Forms.DataGridViewTextBoxColumn direccion_ip;
        private System.Windows.Forms.DataGridViewTextBoxColumn puerto;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn contrasena;
        private System.Windows.Forms.DataGridViewTextBoxColumn estatus;
        private System.Windows.Forms.BindingSource variablesBindingSource2;
        private System.Windows.Forms.BindingSource variablesBindingSource1;
        private System.Windows.Forms.BindingSource variablesBindingSource3;
        private System.Windows.Forms.DataGridView data_user_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_tarjeta;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn cedula_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn puntodeventa_user;
        private System.Windows.Forms.Button button_DeleteUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_usuario_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn process;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_tarjeta_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn cedula_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn fingerprint;
        private System.Windows.Forms.DataGridViewTextBoxColumn huella;
        private System.Windows.Forms.ToolTip toolTip_MessageNewDevice;
        private System.Windows.Forms.CheckedListBox checkedListBox_Processes;
    }
}

