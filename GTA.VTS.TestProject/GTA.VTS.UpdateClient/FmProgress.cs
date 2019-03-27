using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace GIMSClient
{
    public partial class FmProgress : Form
    {
        public delegate void StepOperationRepetition(
            IList listObjToHandle,
            int currentObjPosition,
            out string currentFunInfo,
            params object[] otherParams);
      
        public delegate void StepOperationNoRepetition(
          int stepCount,
          int stepCurrent,
          out string currentFunInfo,
          params object[] otherParams);

        public event EventHandler CancelClick = null;
        public StepOperationRepetition stepOperation;
        public StepOperationRepetition StepOperation
        {
            get
            {
                return stepOperation;
            }
            set
            {
                stepOperation = value;
            }
        }
        public StepOperationNoRepetition stepNoOperation;
        public StepOperationNoRepetition StepNoOperation
        {
            get
            {
                return stepNoOperation;
            }
            set
            {
                stepNoOperation = value;
            }
        }

        public void ExeRepetition(
            IList listObjToHandle,
            int currentObjPosition,
            string currentFunInfo,
            params object[] otherParams)
        {
            Show();
            PMaxValue = listObjToHandle.Count;
            PValue = 0;
            for (int i = 0; i < listObjToHandle.Count; i++)
            {
                StepOperation.Invoke(listObjToHandle, i, out currentFunInfo, otherParams);

                Text = currentFunInfo;
                PValue = i + 1;
                Refresh();
            }
        }

        public void ExeNoRepetition(
           int stepCount,
           int currentObjPosition,
           string currentFunInfo,
           params object[] otherParams)
        {
            Show();
            PMaxValue = stepCount;
            PValue = 0;
            for (int i = 0; i < stepCount; i++)
            {
                StepNoOperation.Invoke(stepCount, i, out currentFunInfo, otherParams);
                Text = currentFunInfo;
                PValue = i + 1;
                Refresh();
            }
        }
        
        public FmProgress()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get { return Text; }
            set { Text = value; }
        }

        public int PValue
        {
            get { return pBar.Value; }
            set 
            {
                if (value < pBar.Maximum)
                {
                    pBar.Value = value;
                    Show();
                }
                else
                {
                    Close();
                }
            }
        }

        public int PMaxValue
        {
            get { return pBar.Maximum; }
            set { pBar.Maximum = value; }
        }

        private void FmProgress_Load(object sender, EventArgs e)
        {
            if (StepOperation == null && StepNoOperation == null)
            {
                throw new ApplicationException("StepOperation Can not be null!");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelClick != null)
                CancelClick(this, new EventArgs());
            Close();
        }
   
    
    }
}